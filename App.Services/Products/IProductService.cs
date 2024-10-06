using App.Service;
using App.Services.Products.Create;
using App.Services.Products.Update;
using App.Services.Products.Update.Stock;
namespace App.Services.Products;

public interface IProductService
{
    Task<ServiceResult<List<ProductDTO>>> GetAllListAsync();

    Task<ServiceResult<List<ProductDTO>>> GetPagedListAsnyc(int pageNumber, int pageSize);
    Task<ServiceResult<List<ProductDTO>>> GetTopPriceProductsAsync(int  ount);

    Task<ServiceResult<List<ProductDTO>>> GetMinStockProducts(int minStockCount);

    Task<ServiceResult<ProductDTO?>> GetByIdAsync(int id);
 
    Task<ServiceResult<CreateProductResponse>> CreateAsync(CreateProductRequest request);


    Task<ServiceResult> UpdateAsync(int id, UpdateProductRequest request);

    Task<ServiceResult> DeleteAsync(int id);

    Task<ServiceResult> UpdateProductStock(UpdateProductStockRequest request);

    

    Task<ServiceResult> EnterDataInStock(int id, int stockCount);

    Task<ServiceResult> UpdatePrice(int id, decimal price);

    Task<ServiceResult<ProductDTO>> GetPriceWithKDV(int id);


    

}

