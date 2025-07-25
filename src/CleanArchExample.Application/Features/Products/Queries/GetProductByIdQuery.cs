// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using CleanArchExample.Application.Common.Models;
using CleanArchExample.Application.DTOs;
using MediatR;

namespace CleanArchExample.Application.Features.Products.Queries
{
  public record GetProductByIdQuery(Guid Id) : IRequest<Result<ProductDto>>;

}