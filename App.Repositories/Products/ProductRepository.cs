using Microsoft.EntityFrameworkCore;

namespace App.Repositories.Products;

//Product a özgü sorguları burada yazacağız.

//miras alma işleminde metotlar mirasl alınır, class a özgü context gibi şeyler miras alınmaz.


public class ProductRepository(AppDbContext context) : GenericRepository<Product>(context), IProductRepository//interface i concrete ederken de genelden özele doğru gidilir. Bu sayede sadece ilgili metotumuzu kodumuzda miras alırız ama hepsini almış oluruz
{


    private readonly AppDbContext _context = context;


    public async Task<List<Product>> GetTopPriceProductsAsync(int count)
    {
        return await _context.Products.OrderByDescending(p => p.Price).Take(count).ToListAsync();
    }

}

