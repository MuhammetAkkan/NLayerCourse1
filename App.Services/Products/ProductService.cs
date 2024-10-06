using App.Repositories;
using App.Repositories.Products;
using App.Service;
using App.Services.ExceptionHandler;
using App.Services.Products.Create;
using App.Services.Products.Update;
using App.Services.Products.Update.Stock;
using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Net;
namespace App.Services.Products;

//servce katmnanı genellikle bir repository katmanınından referans ile bir nesne alır ve işlemleri yapar.

public class ProductService(IProductRepository _productRepository, IUnitOfWork unitOfWork, IValidator<CreateProductRequest> createProductValidator, IMapper mapper) : IProductService
{
    /*
     * ProductRepositroyden nesne alıyor çünkü öncelikle datanın gelmesi lazım.
     * 
     */
    
    public async Task<ServiceResult<List<ProductDTO>>> GetAllListAsync()
    {
        #region CriticalExceptionDenemesi=>Off
        //throw new CriticalException("Kritik bir hata meydana geldi.");
        #endregion

        //throw new Exception("standart hata");

        // data katmanından veri çekme işlemi
        var products = await _productRepository.GetAll().ToListAsync();

        #region Manuel_Mapping_Off
        //var productResponse = products.Select(x => new ProductDTO(x.Id, x.Name, x.Price, x.Stock)).ToList();
        #endregion


        #region AutoMapper_is_Working
        //autoMapper kullanarak mapping işlemi => IMapper mapper
        var productAsDto = mapper.Map<List<ProductDTO>>(products);
        #endregion

        // Başarılı sonuç döndürme
        return ServiceResult<List<ProductDTO>>.Success(productAsDto);
    }


    public async Task<ServiceResult<List<ProductDTO>>> GetPagedListAsnyc(int pageNumber, int pageCapacity)
    {

        //skip ve take metotları ile sayfalama işlemi yapılır.
        //skip bize kaçıncı elemandan başlayacağımızı söyler.

        var skipValue = (pageNumber - 1) * pageCapacity;

        var products = await _productRepository.GetAll().Skip(skipValue).Take(pageCapacity).ToListAsync();

        #region manuelMapping
        //var asProductsDto = products.Select(x => new ProductDTO(x.Id, x.Name, x.Price, x.Stock)).ToList();
        #endregion


        var productAsDto = mapper.Map<List<ProductDTO>>(products);

        return ServiceResult<List<ProductDTO>>.Success(productAsDto);

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
        //var productResponse = products.Select(x => new ProductDTO(x.Id, x.Name, x.Price, x.Stock)).ToList();

        var productResponse = mapper.Map<List<ProductDTO>>(products);

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
            return ServiceResult<ProductDTO?>.Fail("Product not found", HttpStatusCode.NotFound);
            
        }

        //var productAsResponse = new ProductDTO(products!.Id, products.Name, products.Price, products.Stock);

        var productAsResponse = mapper.Map<ProductDTO?>(products);

        return ServiceResult<ProductDTO>.Success(productAsResponse!)!;
    }



    public async Task<ServiceResult<CreateProductResponse>> CreateAsync(CreateProductRequest request)
    {
        
        //Aynı isimden ürün eklemesini önlemek, 2.yaklaşım. Bu daha sağlıklı bir yaklaşım.
        var anySameNameProduct = await _productRepository.Where(i => i.Name == request.Name).AnyAsync();

        if(anySameNameProduct is true)
        {
            return ServiceResult<CreateProductResponse>.Fail("Product name is already exist", HttpStatusCode.BadRequest);
        }


        /*
         //3.yol => FluentValidation is used
        var validationResult = createProductValidator.ValidateAsync(request);

        if(!validationResult.Result.IsValid)
        {
            return ServiceResult<CreateProductResponse>.Fail(validationResult.Result.Errors.Select(i => i.ErrorMessage).ToList(), HttpStatusCode.BadRequest);
        }
        */

        #region ManuelMapping
        /* code
        var product = new Product()
        {
            Name = request.Name!,
            Price = request.Price,
            Stock = request.Stock
        };
        */
        #endregion

        #region AutoMapper
        //elimizde bir nesne olmadığından doğrudan generic olarak yazdık.
        var product = mapper.Map<Product>(request);
        #endregion

        await _productRepository.AddAsync(product);

        await unitOfWork.SaveChangesAsync();


        var url = $"api/products/{product.Id}";

        return ServiceResult<CreateProductResponse>.SuccessAsCreated(new CreateProductResponse(product.Id), url);

    }


    public async Task<ServiceResult> UpdateAsync(int id, UpdateProductRequest request)
    {
        #region Product_Any_Control
        var product = await _productRepository.GetByIdAsync(id);

        if (product is null)
        {
            return ServiceResult.Fail("Product is null", HttpStatusCode.BadRequest);
        }
        #endregion

        #region ProductNameValidation
        //ürün ismi başka bir Id ye sahip ürüne de ait olmamalı.
        var isProductNameExist = await _productRepository.Where(i => i.Name == request.Name && i.Id != product.Id).AnyAsync();

        if (isProductNameExist       is true)
        {
            return ServiceResult.Fail("Product name is already exist", HttpStatusCode.BadRequest);
        }
        #endregion

        //stok ve price için UpdateProductRequestValidator oluşturuldu.

        #region ManuelMapping_Off
        /*
        product.Name = request.Name;
        product.Price = request.Price;
        product.Stock = request.Stock;
        */
        #endregion

        #region AutoMapper
        product = mapper.Map(request, product);
        #endregion

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
