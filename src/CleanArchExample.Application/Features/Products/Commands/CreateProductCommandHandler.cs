using System.ComponentModel.DataAnnotations;
using CleanArchExample.Application.Features.Products.Commands;
using CleanArchExample.Application.Features.Products.Validators;
using CleanArchExample.Domain.Entities;
using CleanArchExample.Domain.Interfaces;
using MediatR;

namespace CleanArchExample.Application.Features.Products.Handlers
{
    public class CreateProductHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateProductCommandValidator();
            var result = validator.Validate(request);
            if (!result.IsValid)
                throw new ValidationException();
            var entity = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Product.Name,
                Price = request.Product.Price,
                Stock = request.Product.Stock
            };

            await _unitOfWork.Repository<Product>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return entity.Id;
        }
    }
}