using CleanArchExample.Application.Common.Models;
using CleanArchExample.Application.DTOs;
using CleanArchExample.Application.Features.Products.Queries;
using CleanArchExample.Domain.Entities;
using CleanArchExample.Domain.Interfaces;
using MediatR;

namespace CleanArchExample.Application.Features.Products.Handlers;

public class GetAllProductsHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllProductsQuery, PagedResult<ProductDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<PagedResult<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _unitOfWork.Repository<Product>().GetAllAsync();

        var items = products.Select(p => new ProductDto
        {
            Name = p.Name,
            Price = p.Price,
            Stock = p.Stock
        }).ToList();

        return new PagedResult<ProductDto>
        {
            Items = items,
            TotalCount = items.Count,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}
