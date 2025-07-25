// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using AutoMapper;
using CleanArchExample.Application.Common.Models;
using CleanArchExample.Application.DTOs;
using CleanArchExample.Application.Features.Products.Queries;
using CleanArchExample.Domain.Entities;
using CleanArchExample.Domain.Interfaces;
using CleanArchExample.Application.Interfaces.Services;
using MediatR;

namespace CleanArchExample.Application.Features.Products.Handlers
{
    public class GetProductByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cache) : IRequestHandler<GetProductByIdQuery, Result<ProductDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        private readonly ICacheService _cache = cache;

        public async Task<Result<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var key = $"product-{request.Id}";
            var cached = _cache.Get<ProductDto>(key);
            if (cached != null) return Result<ProductDto>.Success(cached);

            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(request.Id);
            if (product == null) return Result<ProductDto>.Failure("Product not found");

            var dto = _mapper.Map<ProductDto>(product);
            _cache.Set(key, dto);

            return Result<ProductDto>.Success(dto);
        }
    }
}