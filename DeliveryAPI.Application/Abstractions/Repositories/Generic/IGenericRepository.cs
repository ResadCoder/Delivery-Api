using System.Linq.Expressions;
using DeliveryAPi.Domain.Entities.Base;

namespace DeliveryAPI.Application.Abstractions.Repositories.Generic;

public interface IGenericRepository<T> where T : BaseEntity , new()
{
    Task CreateAsync(T entity,CancellationToken cancellationToken = default);
    void Update(T entity);
    void Remove(T entity);
    Task<int> GetCountAsync(CancellationToken cancellationToken = default);
    
    Task<T?> GetByIdAsync(
        int id,
        bool isTracking = false,
        CancellationToken cancellationToken = default,
        params string[]? includes);
    
    
    Task<ICollection<T>> GetAllAsync(
        int? page = null,
        int? take = null,
        bool isTracking = false,
        CancellationToken cancellationToken = default,
        params string[]? includes);
    
    Task<T?> GetAnyAsync(
        Expression<Func<T, bool>> predicate,
        bool isTracking = false,
        CancellationToken cancellationToken = default,
        params string[]? includes);
    
    IQueryable<T> GetById(
        int id,
        bool isTracking = false,
        params string[]? includes);
    
    IQueryable<T> GetAny(
        Expression<Func<T, bool>> predicate,
        bool isTracking = false,
        params string[]? includes);

    IQueryable<T> GetAll(
        int? skip = null,
        int? take = null,
        bool isTracking = false,
        params string[]? includes
    );

    Task<bool> isExistAsync(Expression<Func<T, bool>> predicate,CancellationToken cancellationToken = default);
}