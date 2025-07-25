// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using CleanArchExample.Domain.Interfaces;

namespace CleanArchExample.Domain.Entities
{
    public class Product : IAuditableEntity
    {
        public Guid Id { get; set; } // Guid để đảm bảo unique trong distributed system
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

        public void AddStock(int quantity)
        {
            if (quantity <= 0) throw new ArgumentException("Quantity must be positive.");
            Stock += quantity;
        }

        public void RemoveStock(int quantity)
        {
            if (quantity > Stock) throw new InvalidOperationException("Not enough stock.");
            Stock -= quantity;
        }

        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}