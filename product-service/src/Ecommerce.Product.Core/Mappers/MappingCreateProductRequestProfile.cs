using AutoMapper;
using Ecommerce.Product.Core.DTOs;
using Ecommerce.Product.Infrastructure.Entities;

namespace Ecommerce.Product.Core.Mappers;


public class MappingCreateProductRequestProfile : Profile
{
    public MappingCreateProductRequestProfile()
    {
        CreateMap<CreateProductRequest, Products>()
        .ForMember(d => d.ProductID, opt => opt.Ignore())
        .ForMember(d => d.ProductName, opt => opt.MapFrom(s => s.ProductName))
        .ForMember(d => d.Category, opt => opt.MapFrom(s => s.Category))
        .ForMember(d => d.UnitPrice, opt => opt.MapFrom(s => s.UnitPrice))
        .ForMember(d => d.QuantityInStock, opt => opt.MapFrom(s => s.QuantityInStock));
    }
};