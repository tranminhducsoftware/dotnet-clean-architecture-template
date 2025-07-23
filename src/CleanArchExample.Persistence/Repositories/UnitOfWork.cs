using CleanArchExample.Domain.Interfaces;
using CleanArchExample.Persistence.Contexts;

namespace CleanArchExample.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly Dictionary<Type, object> _repositories = new();

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IRepository<T> Repository<T>() where T : class
    {
        var type = typeof(T);
        if (!_repositories.TryGetValue(type, out object? value))
        {
            var repoInstance = new Repository<T>(_context);
            value = repoInstance;
            _repositories.Add(type, value);
        }

        return (IRepository<T>)value;
    }

    public async Task<int> SaveChangesAsync()
        => await _context.SaveChangesAsync();

    public void Dispose()
    {
        _context.Dispose();
    }
}