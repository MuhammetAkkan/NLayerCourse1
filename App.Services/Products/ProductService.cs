using App.Repositories.Products;
using Microsoft.EntityFrameworkCore;
using System.Net;
namespace App.Services.Products;

//servce katmnanı genellikle bir repository katmanınından referans ile bir nesne alır ve işlemleri yapar.
public class ProductService(IProductRepository productRepository) : IProductService
{
    
    public async Task<ServiceResult<List<ProductResponse>>> GetTopPriceProductsAsync(int count)
    {
        // data katmanından veri çekme işlemi
        var products = await productRepository.GetTopPriceProductsAsync(count);

        // Eğer products null ise erken dönüş yapalım
        if (products is null || !products.Any())
        {
            return ServiceResult<List<ProductResponse>>.Fail("Products not found", HttpStatusCode.NotFound);
        }

        // Mapping işlemi
        var productResponse = products.Select(x => new ProductResponse(x.Id, x.Name, x.Price, x.Stock)).ToList();

        // Başarılı sonuç döndürme
        return ServiceResult<List<ProductResponse>>.Success(productResponse);
    }



    //birden falza nesne döndüğünden ProductResponse için mapleme yapılmıştır.
    public async Task<ServiceResult<List<ProductResponse>>> GetMinStockProducts(int minStockCount)
    {
        var products = await productRepository.GetMinStockProducts(minStockCount);

        if (products is null || !products.Any())
        {
            return ServiceResult<List<ProductResponse>>.Fail("Products not found", HttpStatusCode.NotFound);
        }

        var productResponse = products.Select(x => new ProductResponse(x.Id, x.Name, x.Price, x.Stock)).ToList();

        return ServiceResult<List<ProductResponse>>.Success(productResponse);
    }



    //tek bir nesne döndüğünden doğruda ProductResponse döndürülmüştür.
    public async Task<ServiceResult<ProductResponse>> GetProductByIdAsync(int id)
    {
        var products = await productRepository.GetByIdAsync(id);

        if(products is null)
        {
            ServiceResult<ProductResponse>.Fail("Product not found", HttpStatusCode.NotFound);
        }

        var productAsResponse = new ProductResponse(products!.Id, products.Name, products.Price, products.Stock);

        return ServiceResult<ProductResponse>.Success(productAsResponse);
    }

    


}

