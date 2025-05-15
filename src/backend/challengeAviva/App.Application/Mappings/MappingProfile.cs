using Req = App.Core.Dto.Request;
using App.Core.Entities;
using Enums = App.Core.Enums;
using AutoMapper;

namespace App.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Req.OrderRequestDto, Order>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Products, opt => opt.Ignore())
            .ForMember(dest => dest.ProviderKey, opt => opt.Ignore())
            .ForMember(dest => dest.Fee, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => Enums.OrderStatus.Created))
            .ForMember(dest => dest.PaymentMode, opt => opt.MapFrom(src => (Enums.PaymentMode)src.PaymentMode));
        }
    }
}