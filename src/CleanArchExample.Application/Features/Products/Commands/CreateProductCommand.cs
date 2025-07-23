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