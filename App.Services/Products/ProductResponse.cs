﻿namespace App.Services.Products;

public record ProductResponse(int Id, string Name, decimal Price, int Stock);
//şuan ise sadece yukarıdaki gibi tasarlamamız yeterli.

    /*
     * Eski bir yöntem olarak aşağıdaki gibi tasarlardık.
    public int Id { get; init; }
    public string Name { get; init; } = default!;
    public decimal Price { get; init; }
    public int Stock { get; init; }
    
    */

