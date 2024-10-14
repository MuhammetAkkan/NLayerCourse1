
using App.Repositories.Categories;
using App.Services.Categories.Create;
using App.Services.Categories.Dtos;
using App.Services.Categories.Update;
using AutoMapper;

namespace App.Services.Categories.AutoMapper;

public class CategoryMappingProfile : Profile
{
    public CategoryMappingProfile()
    {
        CreateMap<Category, CategoryDto>().ReverseMap(); //Category to CategoryDTO && CategoryDTO to Category

        CreateMap<Category, CategoryWithProductsDTO>(); //Category to CategoryWithProductsDTO && CategoryWithProductsDTO to Category

        //inputtan gelen name i toLower luyoruz, ve bu bir reverse değil tek taraflı bir mapping
        CreateMap<CreateCategoryRequest, Category>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name!.ToLowerInvariant()));

        //inputtan gelen name i toLower luyoruz, ve bu bir reverse değil tek taraflı bir mapping
        CreateMap<UpdateCategoryRequest, Category>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name!.ToLowerInvariant()));

        
    


    }
}
