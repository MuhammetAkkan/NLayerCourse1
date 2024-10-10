using App.Services.ExceptionHandler;
using App.Services.Products;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using App.Services.Categories;

namespace App.Services.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IProductService, ProductService>();

        services.AddScoped<ICategoryService, CategoryService>();

        //fluent validation ekledik.
        services.AddFluentValidationAutoValidation();

        //
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddAutoMapper(Assembly.GetExecutingAssembly()); //AutoMapper ekledik.

        services.AddExceptionHandler<CriticalExceptionHandler>(); //exceptionHandler ekledik.
        services.AddExceptionHandler<GlobalExceptionHandler>();

        return services;
        // Add your repository extensions here

    }
}


