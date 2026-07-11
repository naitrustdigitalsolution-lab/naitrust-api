using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Infrastructure.Data.Extension;
using Naitrust.Infrastructure.Data.Interfaces;

namespace Naitrust.Infrastructure.Data.Implementations;

public class Repository<T> : IRepository<T> where T : class
{
    private bool disposedValue = false;
    protected readonly DbContext _dbContext;
    protected readonly DbSet<T> _dbSet;

    public Repository(DbContext context)
    {
        _dbContext = context ?? throw new ArgumentException(null, nameof(context));
        _dbSet = _dbContext.Set<T>();
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
    {
        return predicate is null ? await _dbSet.CountAsync() : await _dbSet.CountAsync(predicate);
    }

    public async Task<decimal> SumAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal>> selector)
    {
        return await _dbSet.Where(predicate).SumAsync(selector);
    }

    public virtual T Add(T obj)
    {
        try
        {
            _dbSet.Add(obj);
            return obj;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public virtual async Task<T> AddAsync(T obj)
    {
        Add(obj);
        await SaveAsync();
        return obj;
    }

    public virtual void AddRange(IEnumerable<T> records)
    {
        try
        {
            _dbSet.AddRange(records);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public virtual async Task AddRangeAsync(IEnumerable<T> records)
    {
        AddRange(records);
        await SaveAsync();
    }

    public virtual bool Delete(T obj)
    {
        try
        {
            _dbSet.Remove(obj);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public virtual bool DeleteRange(List<T> objs)
    {
        try
        {
            _dbSet.RemoveRange(objs);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public virtual async Task DeleteRangeAsync(List<T> objs)
    {
        DeleteRange(objs);
        await SaveAsync();
    }

    public virtual bool DeleteRange(Expression<Func<T, bool>> predicate)
    {
        try
        {
            var obj = GetSingleBy(predicate);
            if (obj != null)
            {
                _dbSet.RemoveRange(obj);
                return true;
            }
            else
                throw new Exception("object does not exist");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public virtual async Task DeleteRangeAsync(Expression<Func<T, bool>> predicate)
    {
        DeleteRange(predicate);
        await SaveAsync();
    }

    public virtual bool Delete(Expression<Func<T, bool>> predicate)
    {
        try
        {
            var obj = GetSingleBy(predicate);
            if (obj != null)
            {
                _dbSet.Remove(obj);
                return true;
            }
            else
                throw new Exception("object does not exist");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public virtual async Task DeleteAsync(T obj)
    {
        Delete(obj);
        await SaveAsync();
    }

    public virtual async Task DeleteAsync(Expression<Func<T, bool>> predicate)
    {
        Delete(predicate);
        await SaveAsync();
    }

    public virtual bool DeleteById(object id)
    {
        try
        {
            var obj = _dbSet.Find(id);
            if (obj != null)
            {
                _dbSet.Remove(obj);
                return true;
            }
            else
                throw new Exception($"object with id {id} does not exist");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public virtual async Task DeleteByIdAsync(object id)
    {
        DeleteById(id);
        await SaveAsync();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
    {
        try
        {
            IQueryable<T> query = ConstructQueryInternal(orderBy, include);
            return await query.ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public virtual async Task<IEnumerable<T>> GetAllDataAsync(
        Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
    {
        IQueryable<T> query = _dbSet;

        if (predicate != null)
            query = query.Where(predicate);

        if (include != null)
            query = include(query);

        if (orderBy != null)
            query = orderBy(query);

        return await query.ToListAsync();
    }

    public virtual async Task<Paginated<T>> GetAllPaginatedAsync(
        int page = 1,
        int pageSize = 15,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
    {
        try
        {
            IQueryable<T> query = ConstructQueryInternal(orderBy, include);
            return await query.PaginateAsync(page, pageSize);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public virtual async Task<Paginated<T>> GetAllPaginatedAsync(
        Func<IQueryable<T>, IQueryable<T>>? filter = null,
        int page = 1,
        int pageSize = 15,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
    {
        try
        {
            IQueryable<T> query = ConstructQueryWithFilter(filter, orderBy, include);
            return await query.PaginateAsync(page, pageSize);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error fetching paginated {typeof(T).Name}: {ex.Message}", ex);
        }
    }

    public virtual async Task<T?> GetSingleByAsync(
        Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        int? skip = null,
        int? take = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
        bool tracking = false)
    {
        try
        {
            IQueryable<T> query = ConstructQuery(predicate, orderBy, skip, take, include);
            if (!tracking)
                return await query.AsNoTracking().FirstOrDefaultAsync();
            return await query.FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public virtual T? GetById(object id)
    {
        return _dbSet.Find(id);
    }

    public virtual async Task<T?> GetByIdAsync(object id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<PaginationResult<T>> GetPagedItems(
        RequestParameters parameters,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy,
        Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
        bool disableTracking = true)
    {
        int skip = (parameters.PageNumber - 1) * parameters.PageSize;
        int take = parameters.PageSize;
        var totalRecords = predicate != null
            ? await _dbSet.CountAsync(predicate)
            : await _dbSet.CountAsync();
        List<T> items = await ConstructQueryWithTracking(predicate, orderBy, skip, take, include, disableTracking).ToListAsync();
        return new PaginationResult<T>
        {
            PageSize = parameters.PageSize,
            TotalPages = (int)Math.Ceiling((double)totalRecords / parameters.PageSize),
            CurrentPage = parameters.PageNumber,
            Data = items,
            TotalRecords = totalRecords,
        };
    }

    public virtual T? GetSingleBy(Expression<Func<T, bool>> predicate)
    {
        return _dbSet.FirstOrDefault(predicate);
    }

    public virtual async Task<T?> GetSingleByAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
    }

    public virtual int Save()
    {
        try
        {
            return _dbContext.SaveChanges();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public virtual Task<int> SaveAsync()
    {
        try
        {
            return _dbContext.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public virtual T Update(T obj)
    {
        try
        {
            _dbSet.Attach(obj);
            _dbContext.Entry(obj).State = EntityState.Modified;
            return obj;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public virtual async Task<T> UpdateAsync(T obj)
    {
        Update(obj);
        await SaveAsync();
        return obj;
    }

    public virtual void UpdateRange(IEnumerable<T> records)
    {
        try
        {
            _dbSet.UpdateRange(records);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public virtual async Task UpdateRangeAsync(IEnumerable<T> records)
    {
        UpdateRange(records);
        await SaveAsync();
    }

    public IQueryable<T> ConstructQuery(
        Expression<Func<T, bool>>? predicate,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy,
        int? skip,
        int? take,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include)
    {
        IQueryable<T> query = _dbSet;

        if (orderBy != null)
            query = orderBy(query);

        if (include != null)
            query = include(query);

        if (predicate != null)
            query = query.Where(predicate);

        if (skip != null)
            query = query.Skip(skip.Value);

        if (take != null)
            query = query.Take(take.Value);

        return query;
    }

    private IQueryable<T> ConstructQueryInternal(
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include)
    {
        IQueryable<T> query = _dbSet;

        if (orderBy != null)
            query = orderBy(query);

        if (include != null)
            query = include(query);

        return query;
    }

    private IQueryable<T> ConstructQueryWithFilter(
        Func<IQueryable<T>, IQueryable<T>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
    {
        IQueryable<T> query = _dbSet;

        try
        {
            if (filter != null)
                query = filter(query);

            if (include != null)
                query = include(query);

            if (orderBy != null)
                query = orderBy(query);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error constructing query for {typeof(T).Name}: {ex.Message}", ex);
        }

        return query;
    }

    private IQueryable<T> ConstructQueryWithTracking(
        Expression<Func<T, bool>>? predicate,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy,
        int? skip,
        int? take,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include,
        bool disableTracking)
    {
        IQueryable<T> query = _dbSet;

        if (disableTracking)
            query = query.AsNoTracking();

        if (predicate != null)
            query = query.Where(predicate);

        if (orderBy != null)
            query = orderBy(query);

        if (include != null)
            query = include(query);

        if (skip != null)
            query = query.Skip(skip.Value);

        if (take != null)
            query = query.Take(take.Value);

        return query;
    }
}
