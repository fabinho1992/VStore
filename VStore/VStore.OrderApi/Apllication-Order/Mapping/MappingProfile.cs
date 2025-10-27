using AutoMapper;
using VStore.OrderApi.Apllication_Order.Dtos.Response;
using VStore.OrderApi.Domain.Models;

namespace VStore.OrderApi.Apllication_Order.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Order, OrderResponse>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.Items))
                .ReverseMap();

            CreateMap<OrderItem, OrderItemResponse>()
                .ReverseMap();
        }
    }
}
