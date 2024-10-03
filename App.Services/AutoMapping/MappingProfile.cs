using App.Repositories.Products;
using App.Services.Products;
using AutoMapper;

namespace App.Services.AutoMapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDTO>().ReverseMap(); //Product to ProductDTO && ProductDTO to Product


    }
}
