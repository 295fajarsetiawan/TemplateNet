using System.Linq.Expressions;
using Core;
using Core.DTO;
using Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Data.Base;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntities
{
    protected ConnectionDbContexts Context { get; set; }
    protected DbSet<T> Database { get; set; }

    public BaseRepository(ConnectionDbContexts context)
    {
        Context = context;
        Database = context.Set<T>();
    }

    public virtual void Add(T entity)
    {
        if (entity.HasProp("CreatedAt"))
        {
            entity.GetType().GetProperty("CreatedAt")?.SetValue(entity, DateTimeOffset.UtcNow);
            entity.GetType().GetProperty("UpdatedAt")?.SetValue(entity, DateTimeOffset.UtcNow);
        }

        Database.Add(entity);
    }

    public virtual T GetById(long id)
    {
        return Database.Find(id);
    }

    public virtual T GetById(string id)
    {
        return Database.Find(id);
    }

    public virtual T? GetById(string id, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = Database;

        // Apply includes if any
        if (includes != null && includes.Any())
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return query.FirstOrDefault(e => EF.Property<string>(e, "Id") == id);
    }

    public async Task<T> GetByIdAsync(string id)
    {
        return await Database.FindAsync(id);
    }

    public T GetById(Guid id, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = Database;
        if (includes != null && includes.Any())
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return query.FirstOrDefault(e => e.Id == id && e.DeletedAt == null);
    }

    public T GetByIdNoTracking(Guid id)
    {
        return Database.AsNoTracking().FirstOrDefault(e => EF.Property<Guid>(e, "Id") == id);
    }


    public virtual IDbContextTransaction BeginTransaction()
    {
        return Context.Database.BeginTransaction();
    }

    public virtual IQueryable<T> GetAll()
    {
        return Database;
    }

    public virtual int SaveChanges()
    {
        return Context.SaveChanges();
    }

    public virtual Task<int> SaveChangesAsync()
    {
        return Context.SaveChangesAsync();
    }

    public virtual IQueryable<T> Query()
    {
        return Database;
    }

    public virtual void Remove(T entity)
    {
        Database.Remove(entity);
    }

    public virtual void RemoveById(long id)
    {
        Database.Remove(Database.Find(id));
    }

    public virtual void RemoveById(Guid id)
    {
        Database.Remove(Database.Find(id));
    }

    public virtual void RemoveById(string id)
    {
        Database.Remove(Database.Find(id));
    }

    public virtual void SoftDelete(T entity, string id)
    {
        if (entity.HasProp("DeletedAt"))
        {
            entity.GetType().GetProperty("DeletedAt").SetValue(entity, DateTimeOffset.UtcNow);
        }

        if (entity.HasProp("IsDeleted"))
        {
            entity.GetType().GetProperty("IsDeleted").SetValue(entity, true);
        }

        Database.Update(entity);
    }

    public virtual void SoftDelete(T entity, long id)
    {
        if (entity.HasProp("DeletedAt"))
        {
            entity.GetType().GetProperty("DeletedAt").SetValue(entity, DateTimeOffset.UtcNow);
        }

        if (entity.HasProp("IsDeleted"))
        {
            entity.GetType().GetProperty("IsDeleted").SetValue(entity, true);
        }

        Database.Update(entity);
    }

    public virtual void SoftDelete(T entity, Guid id)
    {
        if (entity.HasProp("DeletedAt"))
        {
            entity.GetType().GetProperty("DeletedAt").SetValue(entity, DateTimeOffset.UtcNow);
        }

        if (entity.HasProp("IsDeleted"))
        {
            entity.GetType().GetProperty("IsDeleted").SetValue(entity, true);
        }

        Database.Update(entity);
    }

    public virtual void Update(T entity)
    {
        if (entity.HasProp("UpdatedAt"))
        {
            entity.GetType().GetProperty("UpdatedAt").SetValue(entity, DateTimeOffset.UtcNow);
        }

        Database.Update(entity);
    }

    public virtual void AddRange(IEnumerable<T> entities)
    {
        foreach (var entity in entities)
        {
            if (entity.HasProp("CreatedAt"))
            {
                entity.GetType().GetProperty("CreatedAt").SetValue(entity, DateTimeOffset.UtcNow);
            }
        }

        Database.AddRange(entities);
    }

    public virtual void RemoveRange(IEnumerable<T> entities)
    {
        Database.RemoveRange(entities);
    }

    public async Task<PagedResult<T>> GetPagedAsync(PaginationParams<T> paginationParams)
    {
        var query = Database.AsQueryable();
        if (paginationParams.Filter != null)
        {
            query = query.Where(paginationParams.Filter);
        }

        if (typeof(T).GetProperty("DeletedAt") != null)
        {
            query = query.Where(x =>
                EF.Property<DateTimeOffset?>(x, "DeletedAt") == null);
        }

        if (!string.IsNullOrEmpty(paginationParams.SearchQuery) && paginationParams.Fields != null &&
            paginationParams.Fields.Any())
        {
            var filters = DBQueryFunction.BuildSearchFilter<T>(paginationParams.SearchQuery, paginationParams.Fields);
            query = query.Where(filters);
        }
        
        if (paginationParams.IncludeProperties != null && paginationParams.IncludeProperties.Any())
        {
            foreach (var includeProperty in paginationParams.IncludeProperties)
            {
                var property = typeof(T).GetProperty(includeProperty);
                if (property != null && property.PropertyType.IsClass && property.PropertyType != typeof(string))
                {
                    query = query.Include(includeProperty);
                }
            }
        }

        if (!string.IsNullOrEmpty(paginationParams.OrderByField))
        {
            var param = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(param, paginationParams.OrderByField);
            var lambda = Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), param);

            if (paginationParams.Descending.ToLower() == "desc")
            {
                query = query.OrderByDescending(lambda);
            }
            else
            {
                query = query.OrderBy(lambda);
            }
        }

        int totalRecords = await query.CountAsync();

        var items = await query
            .Skip((paginationParams.Page - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();

        int totalPages = (int)Math.Ceiling((double)totalRecords / paginationParams.PageSize);

        int? previousPage = (paginationParams.Page > 1) ? paginationParams.Page - 1 : (int?)null;
        int? nextPage = (paginationParams.Page < totalPages) ? paginationParams.Page + 1 : (int?)null;

        return new PagedResult<T>
        {
            Results = items,
            TotalCount = totalRecords,
            Page = paginationParams.Page,
            PageSize = paginationParams.PageSize,
            Next = nextPage == null ? null : $"Page={nextPage}",
            Previous = previousPage == null ? null : $"Page={previousPage}"
        };
    }
}