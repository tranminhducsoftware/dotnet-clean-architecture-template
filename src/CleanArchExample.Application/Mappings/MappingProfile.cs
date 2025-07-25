// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using AutoMapper;
using CleanArchExample.Application.DTOs;
using CleanArchExample.Domain.Entities;

namespace CleanArchExample.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
        }
    }
}
