namespace App.Repositories.Products;

//bu interface genel interfacein yapısını alır ve Product a özgü sorguları burada ekleriz.
public interface IProductRepository : IGenericRepository<Product, int>
{
    //Product a özgü sorguları burada yazacağız.
    public Task<List<Product>> GetTopPriceProductsAsync(int count);

    public Task<List<Product>> GetMinStockProducts(int minStockCount);



}

