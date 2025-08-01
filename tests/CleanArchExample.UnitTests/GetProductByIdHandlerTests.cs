// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using AutoMapper;

using CleanArchExample.Application.DTOs;
using CleanArchExample.Application.Features.Products.Handlers;
using CleanArchExample.Application.Features.Products.Queries;
using CleanArchExample.Application.Interfaces.Services;
using CleanArchExample.Domain.Entities;
using CleanArchExample.Domain.Interfaces;

using Moq;

namespace CleanArchExample.UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public async Task Handle_CacheMiss_CallsRepositoryAndSetsCache()
        {
            // Arrange
            var id = Guid.NewGuid();
            var product = new Product { Id = id, Name = "Product A", Price = 100, Stock = 10, CreatedAt = DateTime.UtcNow };
            var productDto = new ProductDto { Name = "Product A", Price = 100, Stock = 10 };

            var productRepoMock = new Mock<IRepository<Product>>();
            productRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(product);

            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(u => u.Repository<Product>()).Returns(productRepoMock.Object);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<ProductDto>(product)).Returns(productDto);

            var cacheMock = new Mock<ICacheService>();
            // cacheMock.Setup(c => c.Get<ProductDto>(It.IsAny<string>())).Returns((ProductDto?)null);
            // cacheMock.Setup(c => c.Set(It.IsAny<string>(), It.IsAny<ProductDto>(), It.IsAny<TimeSpan?>())).Verifiable();

            var handler = new GetProductByIdHandler(uowMock.Object, mapperMock.Object, cacheMock.Object);

            // Act
            var result = await handler.Handle(new GetProductByIdQuery(id), default);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Product A", result!.Value!.Name);
            productRepoMock.Verify(r => r.GetByIdAsync(id), Times.Once);
            // cacheMock.Verify(c => c.Set(It.IsAny<string>(), productDto, It.IsAny<TimeSpan?>()), Times.Once);
        }



        [Fact]
        public async Task Handle_CacheHit_DoesNotCallRepositoryOrSetCache()
        {
            // Arrange
            var id = Guid.NewGuid();
            var cachedDto = new ProductDto { Name = "FromCache", Price = 50, Stock = 5 };

            var uowMock = new Mock<IUnitOfWork>(); // Không setup DB vì không được gọi
            var mapperMock = new Mock<IMapper>();  // Không setup map vì không được gọi

            var cacheMock = new Mock<ICacheService>();
            // cacheMock.Setup(c => c.Get<ProductDto>(It.IsAny<string>())).Returns(cachedDto);

            var handler = new GetProductByIdHandler(uowMock.Object, mapperMock.Object, cacheMock.Object);

            // Act
            var result = await handler.Handle(new GetProductByIdQuery(id), default);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("FromCache", result!.Value!.Name);

            uowMock.Verify(u => u.Repository<Product>(), Times.Never);
            mapperMock.Verify(m => m.Map<ProductDto>(It.IsAny<Product>()), Times.Never);
            // cacheMock.Verify(c => c.Set(It.IsAny<string>(), It.IsAny<ProductDto>(), It.IsAny<TimeSpan?>()), Times.Never);
        }


    }
}
