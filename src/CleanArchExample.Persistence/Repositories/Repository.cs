// Copyright (c) 2025 tranminhducsoftware. Author: Tran Minh Duc. Licensed under MIT.

using CleanArchExample.Domain.Interfaces;
using CleanArchExample.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CleanArchExample.Persistence.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        private readonly DbSet<T> _entities;

        public Repository(AppDbContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(Guid id)
            => await _entities.FindAsync(id);

        public async Task<IEnumerable<T>> GetAllAsync()
            => await _entities.ToListAsync();

        public async Task AddAsync(T entity)
            => await _entities.AddAsync(entity);

        public void Update(T entity)
            => _entities.Update(entity);

        public void Remove(T entity)
            => _entities.Remove(entity);
    }
}