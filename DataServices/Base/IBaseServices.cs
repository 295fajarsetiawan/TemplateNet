using System.Linq.Expressions;
using Core;
using Core.DTO;
using Data.Base;

namespace DataServices.Base;

public interface IBaseServices<T> where T : BaseEntities
{
        BooleanErrorResult<T> GetByKey(long id);
        BooleanErrorResult<T> GetByKey(string id);
        BooleanErrorResult<T> GetByKeyAllData(string id);
        BooleanErrorResult<T> GetByKey(string id, params Expression<Func<T, object>>[] includes);
        Task<BooleanErrorResult<T>> GetByKeyAsync(string id);
        Task<BooleanErrorResult<T>> SoftDeleteAsync(T obj, string id);
        Task<BooleanErrorResult<T>> SoftDeleteAsync(T obj, Guid id);
        BooleanErrorResult<T> GetByKey(Guid id, params Expression<Func<T, object>>[] includes);
        BooleanErrorResult<T> GetByIdNoTracking(Guid id);
        BooleanErrorResult<IQueryable<T>> GetAll();
        Task<BooleanErrorResult<T>> Create(T obj);
        Task<BooleanErrorResult<T>> CreateOrUpdate(T obj, string id);
        Task<BooleanErrorResult<T>> CreateOrUpdate(T obj, Guid id);
        BooleanErrorResult Update(T obj);
        BooleanErrorResult Destroy(long id);
        BooleanErrorResult Destroy(string id);
        BooleanErrorResult Destroy(Guid id);
        Task<BooleanErrorResult<List<T>>> GetByIds(List<string> ids);
        Task<BooleanErrorResult<PagedResult<T>>> GetPagedAsync(PaginationParams<T> paginationParams);
}