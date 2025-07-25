// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using FluentValidation;
using CleanArchExample.Application.Features.Products.Commands;

namespace CleanArchExample.Application.Features.Products.Validators
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Product.Name)
                .NotEmpty().WithMessage("Tên sản phẩm không được để trống")
                .MaximumLength(100);

            RuleFor(x => x.Product.Price)
                .GreaterThan(0).WithMessage("Giá phải lớn hơn 0");

            RuleFor(x => x.Product.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("Tồn kho không được âm");
        }
    }
}
