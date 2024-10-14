using App.Repositories;
using App.Repositories.Categories;
using App.Service;
using App.Services.Categories.Create;
using App.Services.Categories.Dtos;
using App.Services.Categories.Update;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace App.Services.Categories;

public class CategoryService(ICategoryRepository _categoryRepository, IUnitOfWork _unitOfWork, IMapper mapper) : ICategoryService
{
    
    public async Task<ServiceResult<CategoryWithProductsDTO>> GetCategoryWithProductsAsync(int id)
    {
        var category = await _categoryRepository.GetCategoryWithProductsAsync(id);

        if(category == null)
            return ServiceResult<CategoryWithProductsDTO>.Fail("Category not found.", HttpStatusCode.NotFound);

        var categoryWithProducts = mapper.Map<CategoryWithProductsDTO>(category);

        return ServiceResult<CategoryWithProductsDTO>.Success(categoryWithProducts);
    }



    public async Task<ServiceResult<List<CategoryWithProductsDTO>>> GetCategoryWithProductsAsync()
    {
        var categories = await _categoryRepository.GetCategoryWithProducts().ToListAsync();


        var categoryWithProducts = mapper.Map<List<CategoryWithProductsDTO>>(categories);


        if(categoryWithProducts is null || categoryWithProducts.Count == 0)
            return ServiceResult<List<CategoryWithProductsDTO>>.Fail("Mapping is fail", statusCode: HttpStatusCode.NotFound);

        List<CategoryWithProductsDTO> incele = categoryWithProducts;



        return ServiceResult<List<CategoryWithProductsDTO>>.Success(categoryWithProducts);
    }



    public async Task<ServiceResult<int>> CreateCategoryAsync(CreateCategoryRequest request)
    {
        #region SameNameControl
        var anySameNameCategory = await _categoryRepository.Where(i=> i.Name == request.Name).AnyAsync();

        if(anySameNameCategory)
        {
            return ServiceResult<int>.Fail("There is already a category with the same name.", HttpStatusCode.BadRequest);
        }
        #endregion

        var newCategory  = mapper.Map<Category>(request);

        await _categoryRepository.CreateAsync(newCategory);

        var url = $"api/categories/{newCategory.Id}";

        await _unitOfWork.SaveChangesAsync();

        return ServiceResult<int>.SuccessAsCreated(newCategory.Id, url);
    }

    //update
    public async Task<ServiceResult> UpdateCategoryAsync(int id, UpdateCategoryRequest request)
    {
        var category = await _categoryRepository.GetByIdAsync(id);


        if (category == null) 
            return ServiceResult.Fail("Category not found.", HttpStatusCode.NotFound);
        

        var anySameNameCategory = await _categoryRepository.Where(i => i.Name == request.Name && i.Id != id).AnyAsync();

        if (anySameNameCategory)
        {
            return ServiceResult.Fail("There is already a category with the same name.", HttpStatusCode.BadRequest);
        }

        category = mapper.Map(request, category);

        _categoryRepository.Update(category);

        await _unitOfWork.SaveChangesAsync();

        return ServiceResult.Success(HttpStatusCode.NoContent);
    }

    //DELETE 
    public async Task<ServiceResult> DeleteCategoryAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);

        _categoryRepository.Delete(category!);

        await _unitOfWork.SaveChangesAsync();

        return ServiceResult.Success(HttpStatusCode.NoContent);
    }

    public async Task<ServiceResult<List<CategoryDto>>> GetAllListAsync()
    {
        var categories = await _categoryRepository.GetAll().ToListAsync();

        if (categories is null || categories.Count == 0)
            return ServiceResult<List<CategoryDto>>.Fail("Categories not found.", statusCode: HttpStatusCode.NotFound);

        var categoryDtos = mapper.Map<List<CategoryDto>>(categories);

        return ServiceResult<List<CategoryDto>>.Success(categoryDtos);
    }

    public async Task<ServiceResult<CategoryDto?>> GetByIdAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);

        if (category == null)
            return ServiceResult<CategoryDto?>.Fail("Category not found.", statusCode: HttpStatusCode.NotFound);

        var categoryDto = mapper.Map<CategoryDto>(category);

        return ServiceResult<CategoryDto?>.Success(categoryDto);
    }

    
}
