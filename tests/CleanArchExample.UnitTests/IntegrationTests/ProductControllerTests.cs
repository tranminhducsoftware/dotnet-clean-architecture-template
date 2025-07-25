// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using System.Net.Http.Json;

using CleanArchExample.Application.DTOs;
using CleanArchExample.IntegrationTests.Common;

namespace CleanArchExample.IntegrationTests
{
    public class ProductControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public ProductControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateProduct_ReturnsOk()
        {
            var request = new ProductDto
            {
                Name = "Integration Product",
                Price = 123,
                Stock = 10
            };

            var response = await _client.PostAsJsonAsync("/api/Product", request);

            response.EnsureSuccessStatusCode(); // throw nếu không 200
        }
    }
}
