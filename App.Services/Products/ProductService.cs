using App.Repositories;
using App.Repositories.Products;
using App.Service;
using Microsoft.EntityFrameworkCore;
using System.Net;
namespace App.Services.Products;

//servce katmnanı genellikle bir repository katmanınından referans ile bir nesne alır ve işlemleri yapar.
public class ProductService(IProductRepository _productRepository, IUnitOfWork unitOfWork) : IProductService
{
    /*
     * ProductRepositroyden nesne alıyor çünkü öncelikle datanın gelmesi lazım.
     * 
     */
    
    public async Task<ServiceResult<List<ProductDTO>>> GetAllListAsync()
    {
        // data katmanından veri çekme işlemi
        var products = await _productRepository.GetAll().ToListAsync();

        
        // Mapping işlemi
        var productResponse = products.Select(x => new ProductDTO(x.Id, x.Name, x.Price, x.Stock)).ToList();

        // Başarılı sonuç döndürme
        return ServiceResult<List<ProductDTO>>.Success(productResponse);
    }


    public async Task<ServiceResult<List<ProductDTO>>> GetPagedListAsnyc(int pageNumber, int pageCapacity)
    {

        //skip ve take metotları ile sayfalama işlemi yapılır.
        //skip bize kaçıncı elemandan başlayacağımızı söyler.


        var skipValue = (pageNumber - 1) * pageCapacity;

        var products = await _productRepository.GetAll().Skip(skipValue).Take(pageCapacity).ToListAsync();

        var asProductsDto = products.Select(x => new ProductDTO(x.Id, x.Name, x.Price, x.Stock)).ToList();

        return ServiceResult<List<ProductDTO>>.Success(asProductsDto);

    }

    public async Task<ServiceResult<List<ProductDTO>>> GetTopPriceProductsAsync(int count)
    {
        // data katmanından veri çekme işlemi
        var products = await _productRepository.GetTopPriceProductsAsync(count);

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
        var products = await _productRepository.GetMinStockProducts(minStockCount);

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
        var products = await _productRepository.GetByIdAsync(id);

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
            Name = request.Name!,
            Price = request.Price,
            Stock = request.Stock
        };

        await _productRepository.AddAsync(product);

        await unitOfWork.SaveChangesAsync();


        var url = $"api/products/{product.Id}";


        return ServiceResult<CreateProductResponse>.SuccessAsCreated(new CreateProductResponse(product.Id), url);

    }


    public async Task<ServiceResult> UpdateAsync(int id, UpdateProductRequest request)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product is null)
        {
            return ServiceResult.Fail("Product is null", HttpStatusCode.BadRequest);
        }

        product.Name = request.Name;
        product.Price = request.Price;
        product.Stock = request.Stock;


        _productRepository.Update(product);

        await unitOfWork.SaveChangesAsync();

        return ServiceResult.Success(HttpStatusCode.NoContent);
    }


    public async Task<ServiceResult> DeleteAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if(product is null)
        {
            return ServiceResult.Fail("Product is null", HttpStatusCode.BadRequest);
        }

        _productRepository.Delete(product);

        await unitOfWork.SaveChangesAsync();

        return ServiceResult.Success(HttpStatusCode.NoContent);

    }


    public async Task<ServiceResult> UpdateProductStock(UpdateProductStockRequest request)
    {
        var product = await _productRepository.GetByIdAsync(request.Id);

        if (product is null)
        {
            return ServiceResult.Fail("Product is null", HttpStatusCode.BadRequest);
        }

        product.Stock = request.Stock;

        _productRepository.Update(product);

        await unitOfWork.SaveChangesAsync();

        return ServiceResult.Success(HttpStatusCode.NoContent);
    }

    

    public async Task<ServiceResult> EnterDataInStock(int id, int stockCount)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product is null) 
        {
            return ServiceResult.Fail("Product is null", HttpStatusCode.BadRequest);
        }

        product.Stock += stockCount; //stock güncellendi.

        _productRepository.Update(product);

        await unitOfWork.SaveChangesAsync();

        return ServiceResult.Success(HttpStatusCode.NoContent);

    }

    public async Task<ServiceResult> UpdatePrice(int id, decimal price)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product is null)
            return ServiceResult.Fail("Product is null", HttpStatusCode.BadRequest);
        
        product.Price = price;

        _productRepository.Update(product);

        await unitOfWork.SaveChangesAsync();

        return ServiceResult.Success(HttpStatusCode.NoContent);

    }

    public async Task<ServiceResult<ProductDTO>> GetPriceWithKDV(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product is null)
            return ServiceResult<ProductDTO>.Fail("Product is null", HttpStatusCode.BadRequest);


        var priceWithKDV = product.Price * 1.20m;

        return ServiceResult<ProductDTO>.Success(new ProductDTO(product.Id, product.Name, priceWithKDV, product.Stock));

    }

   
}

