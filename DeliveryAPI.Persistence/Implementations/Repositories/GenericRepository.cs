using System.Linq.Expressions;
using DeliveryAPI.Application.Abstractions.Repositories.Generic;
using DeliveryAPi.Domain.Entities.Base;
using DeliveryAPI.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DeliveryAPI.Persistence.Implementations.Repositories;

internal class GenericRepository<T>(AppDbContext context) : IGenericRepository<T>
    where T : BaseEntity, new()
{
    private readonly DbSet<T> _table = context.Set<T>();

    public async Task CreateAsync(T entity,CancellationToken cancellationToken = default)
    {
       await _table.AddAsync(entity, cancellationToken);
    }

    public void Update(T entity)
    {
        _table.Update(entity);
    }

    public void Remove(T entity)
    {
       _table.Remove(entity);
    }

    public async Task<T?> GetByIdAsync(
        int id,
        bool isTracking,
        CancellationToken cancellationToken = default,
        params string[]? includes
        )
    {
        IQueryable<T> query = _table;
        if (!isTracking)
            query = query.AsNoTracking();
        if (includes is not null && includes.Length > 0) 
            query = ApplyIncludes(query, includes);
        
        return await query.FirstOrDefaultAsync(e => e.Id == id, cancellationToken: cancellationToken);
            
    }

    public async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return await _table.CountAsync(cancellationToken: cancellationToken);
    }

    public async Task<ICollection<T>> GetAllAsync(
        int? skip = null,
        int? take = null,
        bool isTracking = false,
        CancellationToken cancellationToken = default,
        params string[]? includes
        )
    {
        IQueryable<T> query = _table;
        if(skip is > 0) 
            query = query.Skip(skip.Value);
        if(take is > 0)
            query = query.Take(take.Value);
        if(!isTracking)
            query = query.AsNoTracking();
        if(includes is not null && includes.Length > 0)
            query = ApplyIncludes(query, includes);
        
        return  await query.ToListAsync(cancellationToken: cancellationToken);
    }
    

    public Task<T?> GetAnyAsync(
        Expression<Func<T, bool>> predicate,
        bool isTracking = false,
        CancellationToken cancellationToken = default,
        params string[]? includes
        )
    {
        IQueryable<T> query = _table;
        if (!isTracking)
            query = query.AsNoTracking();
        if(includes is not null && includes.Length > 0)
            query = ApplyIncludes(query, includes);
        
        return query.FirstOrDefaultAsync(predicate, cancellationToken: cancellationToken);
    }

    public IQueryable<T> GetById(int id, bool isTracking = false, params string[]? includes)
    {
        IQueryable<T> query = _table.Where(t => t.Id == id);
        if (!isTracking)
            query = query.AsNoTracking();
        if (includes is not null && includes.Length > 0) 
            query = ApplyIncludes(query, includes);
        
        return query;

    }

    public IQueryable<T> GetAny(
        Expression<Func<T, bool>> predicate,
        bool isTracking = false,
        params string[]? includes
        )
    {
        IQueryable<T> query = _table.Where(predicate);
        if (!isTracking)
            query = query.AsNoTracking();
        if (includes is not null && includes.Length > 0)
            query = ApplyIncludes(query, includes);
        
        return query;
    }

    public IQueryable<T> GetAll(
        int? skip = null,
        int? take = null,
        bool isTracking = false,
        params string[]? includes)
    {
        IQueryable<T> query = _table;
        if(skip is > 0) 
            query = query.Skip(skip.Value);
        if(take is > 0)
            query = query.Take(take.Value);
        if(!isTracking)
            query = query.AsNoTracking();
        if(includes is not null && includes.Length > 0)
            query = ApplyIncludes(query, includes);
        return query;
    }

    public Task<bool> isExistAsync(Expression<Func<T, bool>> predicate,CancellationToken cancellationToken = default)
    {
        return _table.AnyAsync(predicate, cancellationToken: cancellationToken);
    }

    private static IQueryable<T> ApplyIncludes(IQueryable<T> query, params string[] includes)
    {
        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        return query;
    }
}