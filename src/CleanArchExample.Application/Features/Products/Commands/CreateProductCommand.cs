// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using CleanArchExample.Application.DTOs;
using MediatR;

namespace CleanArchExample.Application.Features.Products.Commands
{
    public class CreateProductCommand : IRequest<Guid>
    {
        public ProductDto Product { get; }

        public CreateProductCommand(ProductDto product)
        {
            Product = product;
        }
    }
}