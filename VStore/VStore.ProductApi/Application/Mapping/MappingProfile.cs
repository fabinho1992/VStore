using AutoMapper;
using VStore.ProductApi.Application.Dtos.Inputs;
using VStore.ProductApi.Application.Dtos.Responses;
using VStore.ProductApi.Domain.Models;

namespace VStore.ProductApi.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Category
            CreateMap<Category, CategoryResponse>().ReverseMap();
            CreateMap<Category, CategoryInput>().ReverseMap();

            //Product
            CreateMap<Product, ProductResponse>().ForMember(dest => dest.CategoryName,
               opt => opt.MapFrom(src => src.Catergory.Name)).ReverseMap();
            CreateMap<Product, ProductInput>().ReverseMap();
        }
    }
}
