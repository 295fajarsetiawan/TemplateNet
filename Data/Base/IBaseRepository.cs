using System.Linq.Expressions;
using Core.DTO;
using Microsoft.EntityFrameworkCore.Storage;

namespace Data.Base;

public interface IBaseRepository<T> where T : BaseEntities
{
    IQueryable<T> Query();

    void Add(T entity);
    void AddRange(IEnumerable<T> entities);
    IDbContextTransaction BeginTransaction();
    int SaveChanges();
    Task<int> SaveChangesAsync();

    T GetById(long id);
    T GetById(string id);
    T? GetById(string id, params Expression<Func<T, object>>[] includes);

    T GetById(Guid id, params Expression<Func<T, object>>[] includes);
    Task<T> GetByIdAsync(string id);
    T GetByIdNoTracking(Guid id);
    
    IQueryable<T> GetAll();
    void Update(T entity);
    
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
    
    void RemoveById(long id);
    void RemoveById(string id);
    void RemoveById(Guid id);
    
    void SoftDelete(T entity, string id);
    void SoftDelete(T entity, long id);
    void SoftDelete(T entity, Guid id);
    Task<PagedResult<T>> GetPagedAsync(PaginationParams<T> paginationParams);
}