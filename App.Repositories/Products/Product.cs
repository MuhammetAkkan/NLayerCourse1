using System.ComponentModel.DataAnnotations;

namespace App.Repositories.Products;

public class Product
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
    public int Stock { get; set; }
}
