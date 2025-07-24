using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchExample.Domain.Entities
{
    public class Product
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
    }
}