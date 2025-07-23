using CleanArchExample.Application.Common.Models;
using CleanArchExample.Application.DTOs;
using MediatR;

namespace CleanArchExample.Application.Features.Products.Queries
{
  public record GetProductByIdQuery(Guid Id) : IRequest<Result<ProductDto>>;

}