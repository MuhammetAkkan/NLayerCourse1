using App.Services.ServiceResult;

namespace App.Services.Products;

public interface IProductService
{
    Task<ServiceResult<List<ProductDTO>>> GetAllListAsync();

    Task<ServiceResult<List<ProductDTO>>> GetPagedListAsnyc(int pageNumber, int pageSize);
    Task<ServiceResult<List<ProductDTO>>> GetTopPriceProductsAsync(int count);

    Task<ServiceResult<List<ProductDTO>>> GetMinStockProducts(int minStockCount);

    Task<ServiceResult<ProductDTO?>> GetByIdAsync(int id);

    Task<ServiceResult<CreateProductResponse>> CreateProductResponse(CreateProductRequest request);


    Task<ServiceResultEmpty> UpdateAsync(int id, UpdateProductRequest request);

    Task<ServiceResultEmpty> DeleteAsync(int id);

}

