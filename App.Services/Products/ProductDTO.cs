namespace App.Services.Products;

public record ProductDTO(int Id, string Name, decimal Price, int Stock, int CategoryId);
//şuan ise sadece yukarıdaki gibi tasarlamamız yeterli.

/*
 * Eski bir yöntem olarak aşağıdaki gibi tasarlardık.
public int Id { get; init; }
public string Name { get; init; } = default!;
public decimal Price { get; init; }
public int Stock { get; init; }

*/

/*
public class ProductDto
{
    public int Id { get; init; }
    public string Name { get; init; } = default!;
    public decimal Price { get; init; }
    public int Stock { get; init; }
}
*/

