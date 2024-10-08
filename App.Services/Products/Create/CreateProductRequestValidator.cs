﻿using App.Repositories.Products;
using App.Services.Products.Create;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace App.Services.Products;

public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    private readonly IProductRepository _productRepository;
    public CreateProductRequestValidator(IProductRepository productRepository)
    {


        RuleFor(i => i.Name)
            .NotNull().WithMessage($"Ürün ismi gereklidir.")
            .NotEmpty().WithMessage($"Ürün ismi boş olamaz gereklidir")
            .Length(1, 100).WithMessage($"Ürün ismi 1 ile 100 karakter arasında olabilir.")
            //.Must(MustUniqueProductName).WithMessage("Ürün ismi veri tabanında bulunmaktadır.");
            ;


        //Price Validation
        RuleFor(i => i.Price)
            .GreaterThan(0).WithMessage("Fiyat 0'dan büyük olmalıdır.")
            .LessThanOrEqualTo(1000000).WithMessage("Fiyat en fazla 1,000,000 olabilir.")
            .NotEmpty().WithMessage("Fiyat boş olamaz.");


        //stock
        RuleFor(i => i.Stock)
            .GreaterThan(-1).WithMessage("Stock negatif olamaz")
            .Must(i => i % 1 == 0).WithMessage("Stock tam sayı olmalıdır.");


        RuleFor(i=> i.CategoryId)
            .GreaterThan(0).WithMessage("Kategori seçiniz.")
            .NotEmpty().WithMessage("Kategori seçiniz.")
            .NotNull().WithMessage("Kategori seçiniz");


    }

    //non-asynchronous method => MustUniqueProductName
    private bool MustUniqueProductName(string name)
    {
        return !(_productRepository.Where(i => i.Name == name).Any());

    }


    //asynchronous method => MustUniqueProductName
    private async Task<bool> MustUniqueProdutName(string name, CancellationToken cancellationToken)
    {
        return ! await (_productRepository.Where(i => i.Name == name).AnyAsync());
    }
}
