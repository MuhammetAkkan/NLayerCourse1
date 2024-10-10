
using Microsoft.EntityFrameworkCore;

namespace App.Repositories.Categories;

public class CategoryRepository(AppDbContext context) : GenericRepository<Category>(context), ICategoryRepository
{
    private readonly AppDbContext _context = context;
    public IQueryable<Category?> GetCategoryWithProducts()
    {
        var productQueryable = _context.Categories
            .Include(i => i.Products)
            .AsNoTracking();

        return productQueryable;
    }

    public async Task<Category?> GetCategoryWithProductsAsync(int id)
    {
        //productsları category ile birlikte getir ve idsi verilen categoryi getir
        var result = await _context.Categories
            .Include(i=> i.Products)
            .FirstOrDefaultAsync(i=> i.Id == id);
        return result;
    }
}
