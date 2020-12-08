using Data.Base;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Shared.Enums.CommonEnum;

namespace Data.Repositories
{
    public class CrudRepository<T> : IReadRepository<T>, IAddRepository<T>, IEditRepository<T>, IDeleteRepository<T> where T : class
    {
        private readonly AppDbContext AppDbContext;

        public CrudRepository(AppDbContext appDbContext)
        {
            AppDbContext = appDbContext;
        }

        #region Get Page
        public async Task<List<T>> GetPageAsync<TKey>(int skipCount, int takeCount, Expression<Func<T, bool>> filter, Expression<Func<T, TKey>> sortingExpression, SortDirectionEnum sortDir = SortDirectionEnum.Ascending, string includeProperties = "")
        {
            IQueryable<T> query = AppDbContext.Set<T>();
            skipCount -= 1;
            skipCount *= takeCount;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            query = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            switch (sortDir)
            {
                case SortDirectionEnum.Ascending:
                    if (skipCount == 0)
                        query = query.OrderBy<T, TKey>(sortingExpression).Take(takeCount);
                    else
                        query = query.OrderBy<T, TKey>(sortingExpression).Skip(skipCount).Take(takeCount);
                    break;
                case SortDirectionEnum.Descending:
                    if (skipCount == 0)
                        query = query.OrderByDescending<T, TKey>(sortingExpression).Take(takeCount);
                    else
                        query = query.OrderByDescending<T, TKey>(sortingExpression).Skip(skipCount).Take(takeCount);
                    break;
                default:
                    break;
            }
            return await query.AsNoTracking().ToListAsync();

        }
        #endregion

        #region GetAll
        public async Task<List<T>> GetAllAsync()
        {
            return await AppDbContext.Set<T>().AsNoTracking().ToListAsync();
        }
        #endregion

        #region First Or Default
        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = AppDbContext.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter, string includeProperties = "")
        {
            IQueryable<T> query = AppDbContext.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            query = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return await query.FirstOrDefaultAsync();
        }
        #endregion

        #region Get Where
        public async Task<List<T>> GetWhereAsync(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = AppDbContext.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public async Task<List<T>> GetWhereAsync(Expression<Func<T, bool>> filter = null, string includeProperties = "")
        {
            IQueryable<T> query = AppDbContext.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            query = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return await query.ToListAsync();
        }

        #endregion

        #region Get Any
        public async Task<bool> GetAnyAsync(Expression<Func<T, bool>> filter = null)
        {
            return await AppDbContext.Set<T>().AnyAsync(filter);
        }
        #endregion

        #region Get Count
        public async Task<int> GetCountAsync()
        {
            return await AppDbContext.Set<T>().CountAsync();
        }
        public async Task<int> GetCountAsync(Expression<Func<T, bool>> filter)
        {
            return await AppDbContext.Set<T>().Where(filter).CountAsync();
        }
        #endregion

        #region Create 
        public void CreateAsyn(T entity)
        {
            AppDbContext.Set<T>().AddAsync(entity);
        }
        public void CreateListAsyn(List<T> entityList)
        {
            AppDbContext.Set<T>().AddRangeAsync(entityList);
        }
        #endregion

        #region Update
        public void Update(T entity)
        {
            AppDbContext.Set<T>().Update(entity);
        }
        public void UpdateList(List<T> entityList)
        {
            AppDbContext.Set<T>().UpdateRange(entityList);
        }
        #endregion

        #region Delete
        public void Delete(T entity)
        {
            AppDbContext.Set<T>().Remove(entity);
        }
        public void DeleteList(List<T> entityList)
        {
            AppDbContext.Set<T>().RemoveRange(entityList);
        }
        #endregion

    }
}
