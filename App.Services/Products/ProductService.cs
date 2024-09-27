using App.Repositories;
using App.Repositories.Products;
using App.Services.ServiceResult;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
namespace App.Services.Products;

//servce katmnanı genellikle bir repository katmanınından referans ile bir nesne alır ve işlemleri yapar.
public class ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork) : IProductService
{
    /*
     * ProductRepositroyden nesne alıyor çünkü öncelikle datanın gelmesi lazım.
     * 
     */


    public async Task<ServiceResult<List<ProductDTO>>> GetAllListAsync()
    {
        // data katmanından veri çekme işlemi
        var products = await productRepository.GetAll().ToListAsync();

        
        // Mapping işlemi
        var productResponse = products.Select(x => new ProductDTO(x.Id, x.Name, x.Price, x.Stock)).ToList();

        // Başarılı sonuç döndürme
        return ServiceResult<List<ProductDTO>>.Success(productResponse);
    }


    public async Task<ServiceResult<List<ProductDTO>>> GetPagedListAsnyc(int pageNumber, int pageSize)
    {

        //skip ve take metotları ile sayfalama işlemi yapılır.
        //skip bize kaçıncı elemandan başlayacağımızı söyler.


        var skipValue = (pageNumber - 1) * pageSize;

        var products = await productRepository.GetAll().Skip(skipValue).Take(pageSize).ToListAsync();

        var asProductsDto = products.Select(x => new ProductDTO(x.Id, x.Name, x.Price, x.Stock)).ToList();

        return ServiceResult<List<ProductDTO>>.Success(asProductsDto);

    }

    public async Task<ServiceResult<List<ProductDTO>>> GetTopPriceProductsAsync(int count)
    {
        // data katmanından veri çekme işlemi
        var products = await productRepository.GetTopPriceProductsAsync(count);

        // Eğer products null ise erken dönüş yapalım
        if (products is null || !products.Any())
        {
            return ServiceResult<List<ProductDTO>>.Fail("Products not found", HttpStatusCode.NotFound);
        }

        // Mapping işlemi
        var productResponse = products.Select(x => new ProductDTO(x.Id, x.Name, x.Price, x.Stock)).ToList();

        // Başarılı sonuç döndürme
        return ServiceResult<List<ProductDTO>>.Success(productResponse);
    }



    //birden falza nesne döndüğünden ProductResponse için mapleme yapılmıştır.
    public async Task<ServiceResult<List<ProductDTO>>> GetMinStockProducts(int minStockCount)
    {
        var products = await productRepository.GetMinStockProducts(minStockCount);

        if (products is null || !products.Any())
        {
            return ServiceResult<List<ProductDTO>>.Fail("Products not found", HttpStatusCode.NotFound);
        }

        //select yapmamız lazım çünkü birden fazla nesne dönecek. 1 den fazla nesne dönerken mapleme yapmak için Select kullanılır.
        var productResponse = products.Select(x => new ProductDTO(x.Id, x.Name, x.Price, x.Stock)).ToList();

        return ServiceResult<List<ProductDTO>>.Success(productResponse);
    }



    //tek bir nesne döndüğünden doğruda ProductResponse döndürülmüştür.
    public async Task<ServiceResult<ProductDTO?>> GetByIdAsync(int id)
    {
        var products = await productRepository.GetByIdAsync(id);

        if (products is null)
        {
            ServiceResult<ProductDTO>.Fail("Product not found", HttpStatusCode.NotFound);
        }

        var productAsResponse = new ProductDTO(products!.Id, products.Name, products.Price, products.Stock);

        return ServiceResult<ProductDTO>.Success(productAsResponse)!;
    }



    public async Task<ServiceResult<CreateProductResponse>> CreateProductResponse(CreateProductRequest request)
    {
        var product = new Product()
        {
            Name = request.Name,
            Price = request.Price,
            Stock = request.Stock
        };

        await productRepository.AddAsync(product);

        await unitOfWork.SaveChangesAsync();

        return ServiceResult<CreateProductResponse>.Success(new CreateProductResponse(product.Id));

    }


    public async Task<ServiceResultEmpty> UpdateAsync(int id, UpdateProductRequest request)
    {
        var product = await productRepository.GetByIdAsync(id);

        if (product is null)
        {
            return ServiceResultEmpty.Fail("Product is null", HttpStatusCode.BadRequest);
        }

        product.Name = request.Name;
        product.Price = request.Price;
        product.Stock = request.Stock;


        productRepository.Update(product);

        await unitOfWork.SaveChangesAsync();

        return ServiceResultEmpty.Success(HttpStatusCode.NoContent);
    }


    public async Task<ServiceResultEmpty> DeleteAsync(int id)
    {
        var product = await productRepository.GetByIdAsync(id);

        if(product is null)
        {
            return ServiceResultEmpty.Fail("Product is null", HttpStatusCode.BadRequest);
        }

        productRepository.Delete(product);

        await unitOfWork.SaveChangesAsync();

        return ServiceResultEmpty.Success(HttpStatusCode.NoContent);

    }




}

