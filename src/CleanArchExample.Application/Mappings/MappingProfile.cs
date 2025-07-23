using AutoMapper;
using CleanArchExample.Application.DTOs;
using CleanArchExample.Domain.Entities;

namespace CleanArchExample.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>().ReverseMap();
    }
}
