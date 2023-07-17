using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using SchoolManagementSystem.Core.Interfaces;

namespace SchoolManagementSystem.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private bool disposedValue = false;
        private readonly DbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public Repository(DbContext context)
        {
            _dbContext = context ?? throw new ArgumentException(null, nameof(context));
            _dbSet = _dbContext.Set<T>();
        }

        public async Task<List<T>> Where(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
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

        public bool Any(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate == null) return _dbSet.Any();
            return _dbSet.Any(predicate);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate == null) return await _dbSet.AnyAsync();
            return await _dbSet.AnyAsync(predicate);
        }

        public virtual long Count(Expression<Func<T, bool>> predicate = null)
        {
            try
            {
                if (predicate == null)
                    return _dbSet.LongCount();
                return _dbSet.LongCount(predicate);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public virtual async Task<long> CountAsync(Expression<Func<T, bool>> predicate = null)
        {
            try
            {
                if (predicate == null)
                    return await _dbSet.LongCountAsync();
                return await _dbSet.LongCountAsync(predicate);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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
                    throw new Exception($"object does not exist");
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

        public virtual bool DeleteRange(Expression<Func<T, bool>> predicate)
        {
            try
            {
                IEnumerable<T> records = from x in _dbSet.Where<T>(predicate) select x;
                _dbSet.RemoveRange(records);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual bool DeleteRange(IEnumerable<T> records)
        {
            try
            {
                _dbSet.RemoveRange(records);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public virtual async Task DeleteRangeAsync(IEnumerable<T> records)
        {
            DeleteRange(records);
            await SaveAsync();
        }
        public virtual async Task DeleteRangeAsync(Expression<Func<T, bool>> predicate)
        {
            DeleteRange(predicate);
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

        public virtual IEnumerable<T> GetAll(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params string[] includeProperties)
        {
            try
            {
                return _dbSet.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            try
            {
                IQueryable<T> query = ConstructQuery(orderBy, include);
                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public virtual IEnumerable<T> GetBy(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, int? skip = null, int? take = null, params string[] includeProperties)
        {
            try
            {
                IQueryable<T> query = ConstructQuery(predicate, orderBy, skip, take, includeProperties);

                return query.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public virtual async Task<IEnumerable<T>> GetByAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, int? skip = null, int? take = null, params string[] includeProperties)
        {
            try
            {
                IQueryable<T> query = ConstructQuery(predicate, orderBy, skip, take, includeProperties);

                return await query.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<IEnumerable<T>> GetByAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, int? skip = null, int? take = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            try
            {
                IQueryable<T> query = ConstructQuery(predicate, orderBy, skip, take, include);

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public virtual async Task<T> GetSingleByAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, int? skip = null, int? take = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool tracking = false)
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


        public virtual T GetById(object id)
        {
            return _dbSet.Find(id);
        }

        public virtual async Task<T> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<T> GetByNameAsync(object name)
        {
            return await _dbSet.FindAsync(name);
        }

        //public async Task<PagedList<T>> GetPagedItems(RequestParameters parameters, Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        //{
        //    var skip = (parameters.PageNumber - 1) * parameters.PageSize;
        //    var items = await ConstructQueryable(predicate, parameters.OrderBy.ToLower(), skip, parameters.PageSize, include).ToListAsync();
        //    var count = await CountAsync(predicate);
        //    return new PagedList<T>(items, count, parameters.PageNumber, parameters.PageSize);
        //}


        public virtual IQueryable<T> GetQueryable(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, int? skip = null, int? take = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            try
            {
                IQueryable<T> query = _dbSet;
                if (predicate != null)
                    query = _dbSet.Where(predicate);

                if (orderBy != null)
                {
                    query = orderBy(query);
                }

                if (include != null)
                    query = include(query);

                if (take != null && skip != null)
                    return query.Skip(skip.Value).Take(take.Value);
                return query;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public virtual T GetSingleBy(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }

        public virtual async Task<T> GetSingleByAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        public async Task<T> LastAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
       Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool disableTracking = true)
        {
            IQueryable<T> query = _dbSet;
            if (disableTracking) query = query.AsNoTracking();

            if (include != null) query = include(query);

            if (predicate != null) query = query.Where(predicate);

            if (orderBy != null)
                return await orderBy(query).LastOrDefaultAsync();
            return await query.LastOrDefaultAsync();
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

        public async Task<decimal> SumAsync(Expression<Func<T, decimal>> predicate)
        {
            return await _dbSet.SumAsync(predicate);
        }

        public async Task<int> SumAsync(Expression<Func<T, int>> predicate)
        {
            return await _dbSet.SumAsync(predicate);
        }

        public async Task<long> SumAsync(Expression<Func<T, long>> predicate)
        {
            return await _dbSet.SumAsync(predicate);
        }

        public virtual T Update(T obj)
        {
            try
            {
                _dbSet.Attach(obj);
                _dbContext.Entry<T>(obj).State = EntityState.Modified;
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

        private IQueryable<T> ConstructQuery(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, Func<IQueryable<T>, IIncludableQueryable<T, object>> include)
        {
            IQueryable<T> query = _dbSet;

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (include != null) query = include(query);

            return query;
        }

        private IQueryable<T> ConstructQuery(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, int? skip, int? take, params string[] includeProperties)
        {
            IQueryable<T> query = _dbSet;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            for (int i = 0; i < includeProperties.Length; i++)
            {
                query = query.Include(includeProperties[i]);
            }

            if (skip != null)
            {
                query = query.Skip(skip.Value);
            }

            if (take != null)
            {
                query = query.Take(take.Value);
            }

            return query;
        }

        private IQueryable<T> ConstructQuery(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, int? skip, int? take, Func<IQueryable<T>, IIncludableQueryable<T, object>> include)
        {
            IQueryable<T> query = _dbSet;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (include != null) query = include(query);

            if (skip != null)
            {
                query = query.Skip(skip.Value);
            }

            if (take != null)
            {
                query = query.Take(take.Value);
            }

            return query;
        }
    }
}
