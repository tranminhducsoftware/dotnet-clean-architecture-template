// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using CleanArchExample.Application.Common.Models;
using CleanArchExample.Application.DTOs;
using MediatR;

namespace CleanArchExample.Application.Features.Products.Queries
{
    public class GetAllProductsQuery : IRequest<PagedResult<ProductDto>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; }
        public string? Filter { get; set; }
    }
}