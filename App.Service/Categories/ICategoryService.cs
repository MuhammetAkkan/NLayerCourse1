using App.Repositories.Categories;
using App.Service;
using App.Services.Categories.Create;
using App.Services.Categories.Dtos;
using App.Services.Categories.Update;

namespace App.Services.Categories;

public interface ICategoryService
{
    Task<ServiceResult<CategoryWithProductsDTO>> GetCategoryWithProductsAsync(int id);

    Task<ServiceResult<List<CategoryWithProductsDTO>>> GetCategoryWithProductsAsync();

    Task<ServiceResult<int>> CreateCategoryAsync(CreateCategoryRequest request);

    Task<ServiceResult> UpdateCategoryAsync(int id, UpdateCategoryRequest request);

    Task<ServiceResult> DeleteCategoryAsync(int id);

    Task<ServiceResult<List<CategoryDto>>> GetAllListAsync();

    Task<ServiceResult<CategoryDto?>> GetByIdAsync(int id);
}