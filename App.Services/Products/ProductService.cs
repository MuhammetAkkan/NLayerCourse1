using App.Repositories.Products;
using System.Net;
namespace App.Services.Products;

//servce katmnanı genellikle bir repository katmanınından referans ile bir nesne alır ve işlemleri yapar.
public class ProductService(IProductRepository productRepository) : IProductService
{

    //yanlış kullanım
    public async Task<ServiceResult<List<Product>>> GetTopPriceProductsAsync(int count)
    {
        var result = await productRepository.GetTopPriceProductsAsync(count);

        return new ServiceResult<List<Product>>
        {
            Data = result,
            ErrorMessage = null
        };
    }


    //doğru kullanım.
    public async Task<ServiceResult<Product>> GetProductByIdAsync(int id)
    {
        var products = await productRepository.GetByIdAsync(id);

        if(products is null)
        {
            ServiceResult<Product>.Fail("Product not found", HttpStatusCode.NotFound);
        }

        return ServiceResult<Product>.Success(products!);
    }


}

