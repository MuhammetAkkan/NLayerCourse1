using App.Repositories.Products;

namespace App.Services.Categories.Dtos;

public record CategoryWithProductsDTO(int id, string Name, List<Product>? Products);

