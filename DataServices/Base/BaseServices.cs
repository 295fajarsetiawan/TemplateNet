using System.Linq.Expressions;
using System.Net;
using Core;
using Core.DTO;
using Data.Base;
using Microsoft.EntityFrameworkCore;

namespace DataServices.Base;

public abstract class BaseServices<T> : IBaseServices<T> where T : BaseEntities
{
    protected IBaseRepository<T> mRepository;

    public BaseServices(IBaseRepository<T> _repository)
    {
        mRepository = _repository;
    }

    public async Task<BooleanErrorResult<T>> Create(T obj)
    {
        try
        {
            
            mRepository.Add(obj);
            mRepository.SaveChanges();
            return new BooleanErrorResult<T>(true, string.Empty, obj, (int)HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            return new BooleanErrorResult<T>(false, ex.InnerException?.Message ??  ex.Message, null, (int)HttpStatusCode.InternalServerError);
        }
    }

    public async Task<BooleanErrorResult<T>> CreateOrUpdate(T obj, string id)
    {
        T existingEntity = null;
        try
        {
            existingEntity = mRepository.GetById(id);
            if (existingEntity != null)
            {
                mRepository.Update(obj);
            }
            else
            {
                mRepository.Add(obj);
            }

            await mRepository.SaveChangesAsync();
            return new BooleanErrorResult<T>(true, string.Empty, obj);
        }
        catch (Exception ex)
        {
            return new BooleanErrorResult<T>(false, ex.InnerException?.Message ?? ex.Message, null);
        }
    }

    public async Task<BooleanErrorResult<T>> CreateOrUpdate(T obj, Guid id)
    {
        T existingEntity = null;
        try
        {
            existingEntity = mRepository.GetByIdNoTracking(id);
            if (existingEntity != null)
            {
                mRepository.Update(obj);
            }
            else
            {
                mRepository.Add(obj);
            }

            await mRepository.SaveChangesAsync();
            return new BooleanErrorResult<T>(true, string.Empty, obj);
        }
        catch (Exception ex)
        {
            return new BooleanErrorResult<T>(false, ex.InnerException?.Message ?? ex.Message, null);
        }
    }


    public BooleanErrorResult Destroy(long id)
    {
        try
        {
            mRepository.RemoveById(id);
            mRepository.SaveChanges();
            return new BooleanErrorResult(true, string.Empty);
        }
        catch (Exception ex)
        {
            return new BooleanErrorResult(false, ex.Message);
        }
    }

    public BooleanErrorResult Destroy(string id)
    {
        try
        {
            mRepository.RemoveById(id);
            mRepository.SaveChanges();
            return new BooleanErrorResult(true, string.Empty);
        }
        catch (Exception ex)
        {
            return new BooleanErrorResult(false, ex.Message);
        }
    }

    public BooleanErrorResult<IQueryable<T>> GetAll()
    {
        try
        {
            var result = mRepository.GetAll();
            return new BooleanErrorResult<IQueryable<T>>(true, string.Empty, result);
        }
        catch (Exception ex)
        {
            return new BooleanErrorResult<IQueryable<T>>(false, ex.InnerException?.Message ?? ex.Message, null);
        }
    }

    public BooleanErrorResult<T> GetByKey(long id)
    {
        T result = null;
        try
        {
            result = mRepository.GetById(id);
            return new BooleanErrorResult<T>(true, string.Empty, result);
        }
        catch (Exception ex)
        {
            return new BooleanErrorResult<T>(false, ex.InnerException?.Message ?? ex.Message, null);
        }
    }

    public BooleanErrorResult<T> GetByKey(string id)
    {
        try
        {
            T? result = mRepository.GetById(id);

            if (result == null)
            {
                return new BooleanErrorResult<T>(false, $"{typeof(T).Name} Not Found", null,
                    (int)HttpStatusCode.NotFound);
            }

            return new BooleanErrorResult<T>(true, string.Empty, result, (int)HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            return new BooleanErrorResult<T>(false,  ex.InnerException?.Message ?? ex.Message, null, (int)HttpStatusCode.InternalServerError);
        }
    }

    public BooleanErrorResult<T> GetByKeyAllData(string id)
    {
        try
        {
            T? result = mRepository.GetById(id);

            if (result == null)
            {
                return new BooleanErrorResult<T>(false, $"{typeof(T).Name} Not Found", null,
                    (int)HttpStatusCode.NotFound);
            }

            return new BooleanErrorResult<T>(true, string.Empty, result, (int)HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            return new BooleanErrorResult<T>(false,  ex.InnerException?.Message ?? ex.Message, null, (int)HttpStatusCode.InternalServerError);
        }
    }

    public BooleanErrorResult<T> GetByKey(string id, params Expression<Func<T, object>>[] includes)
    {
        try
        {
            T? result = mRepository.GetById(id, includes);

            if (result == null)
            {
                return new BooleanErrorResult<T>(false, $"{typeof(T).Name} Not Found", null,
                    (int)HttpStatusCode.NotFound);
            }

            return new BooleanErrorResult<T>(true, string.Empty, result, (int)HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            return new BooleanErrorResult<T>(false,  ex.InnerException?.Message ?? ex.Message, null, (int)HttpStatusCode.InternalServerError);
        }
    }

    public async Task<BooleanErrorResult<T>> GetByKeyAsync(string id)
    {
        try
        {
            T? result = await mRepository.GetByIdAsync(id);

            if (result == null)
            {
                return new BooleanErrorResult<T>(false, $"{typeof(T).Name} Not Found", null,
                    (int)HttpStatusCode.NotFound);
            }

            return new BooleanErrorResult<T>(true, string.Empty, result, (int)HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            return new BooleanErrorResult<T>(false,  ex.InnerException?.Message ?? ex.Message, null, (int)HttpStatusCode.InternalServerError);
        }
    }

    public BooleanErrorResult<T> GetByKey(Guid id,  params Expression<Func<T, object>>[] includes)
    {
        T result = null;
        try
        {
            result = mRepository.GetById(id, includes);
            if(result == null)
            {
                return new BooleanErrorResult<T>(false, "Record Not Found", null, errorCode: 404);
            }
            
            return new BooleanErrorResult<T>(true, string.Empty, result);
        }
        catch (Exception ex)
        {
            return new BooleanErrorResult<T>(false,  ex.InnerException?.Message ?? ex.Message, null, errorCode: 500);
        }
    }

    public BooleanErrorResult<T> GetByIdNoTracking(Guid id)
    {
        T result = null;
        try
        {
            result = mRepository.GetByIdNoTracking(id);
            if (result == null)
            {
                return new BooleanErrorResult<T>(false, "Record Not Found", null);
            }

            return new BooleanErrorResult<T>(true, string.Empty, result);
        }
        catch (Exception ex)
        {
            return new BooleanErrorResult<T>(false,  ex.InnerException?.Message ?? ex.Message, null);
        }
    }

    virtual public BooleanErrorResult Update(T obj)
    {
        try
        {
           var result = mRepository.GetByIdNoTracking(obj.Id);
            if (result == null)
            {
                return new BooleanErrorResult<T>(false, "Record Not Found", null, 404);
            }
            
            mRepository.Update(obj);
            mRepository.SaveChanges();
            return new BooleanErrorResult(true, string.Empty);
        }
        catch (Exception ex)
        {
            return new BooleanErrorResult(false,  ex.InnerException?.Message ?? ex.Message);
        }
    }

    public virtual async Task<BooleanErrorResult<List<T>>> GetByIds(List<string> ids)
    {
        try
        {
            // Parameter for the lambda expression: "s"
            var parameter = Expression.Parameter(typeof(T), "s");

            // Access the "Id" property on s: s.Id
            var idProperty = Expression.Property(parameter, "Id");

            // Create a constant expression for the ids list
            var idsConstant = Expression.Constant(ids);

            // Get the "Contains" method from List<string>
            var containsMethod = typeof(List<string>).GetMethod("Contains", [typeof(string)]);

            // Build the method call expression: ids.Contains(s.Id)
            var containsCall = Expression.Call(idsConstant, containsMethod, idProperty);

            // Create the complete lambda: s => ids.Contains(s.Id)
            var lambda = Expression.Lambda<Func<T, bool>>(containsCall, parameter);

            // Use the lambda in your query
            var getData = await mRepository.Query().Where(lambda).ToListAsync();

            return new BooleanErrorResult<List<T>>(true, string.Empty, getData, 200);
        }
        catch (Exception ex)
        {
            return new BooleanErrorResult<List<T>>(false, ex.Message, null, 500);
        }
    }

    public async Task<BooleanErrorResult<T>> SoftDeleteAsync(T obj, string id)
    {
        try
        {
            mRepository.SoftDelete(obj, id);
            await mRepository.SaveChangesAsync();
            return new BooleanErrorResult<T>(true, "Success", obj, 200);
        }
        catch (Exception ex)
        {
            return new BooleanErrorResult<T>(false,  ex.InnerException?.Message ?? ex.Message, null, 500);
        }
    }

    public async Task<BooleanErrorResult<T>> SoftDeleteAsync(T obj, Guid id)
    {
        try
        {
            mRepository.SoftDelete(obj, id);
            await mRepository.SaveChangesAsync();
            return new BooleanErrorResult<T>(true, "Success", obj, 200);
        }
        catch (Exception ex)
        {
            return new BooleanErrorResult<T>(false,  ex.InnerException?.Message ?? ex.Message, null!, 500);
        }
    }

    public BooleanErrorResult Destroy(Guid id)
    {
        try
        {
            mRepository.RemoveById(id);
            mRepository.SaveChanges();
            return new BooleanErrorResult(true, string.Empty);
        }
        catch (Exception ex)
        {
            return new BooleanErrorResult(false, ex.Message, null!, 500);
        }
    }
    
    public async Task<BooleanErrorResult<PagedResult<T>>>  GetPagedAsync(PaginationParams<T> paginationParams)
    {
        try
        {
            var result = mRepository.GetPagedAsync(paginationParams);
            return new BooleanErrorResult<PagedResult<T>>(true, string.Empty, await result, 200);
        }
        catch (Exception ex)
        {
            return new BooleanErrorResult<PagedResult<T>>(false,  ex.InnerException?.Message ?? ex.Message, null!, 500);            
        }
    }
}