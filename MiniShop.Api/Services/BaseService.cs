using Microsoft.EntityFrameworkCore;
using MiniShop.Api.Database;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MiniShop.Api.Services
{
    public abstract class BaseService<T> where T : class
    {
        protected AppDbContext _context;

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

        public bool Delete(T model)
        {
            _context.Entry<T>(model).State = EntityState.Deleted;
            return true;
        }

        public T Insert(T model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            _context.Set<T>().Add(model);
            return model;
        }

        public IQueryable<T> Select(Expression<Func<T, bool>> whereLambda)
        {
            return _context.Set<T>().Where<T>(whereLambda);
        }

        public IQueryable<T> SelectPage<s>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, s>> orderbyLambda, bool isAsc)
        {
            var temp = _context.Set<T>().Where<T>(whereLambda);
            totalCount = temp.Count();
            if (isAsc)
            {
                temp = temp.OrderBy<T, s>(orderbyLambda).Skip<T>((pageIndex - 1) * pageSize).Take<T>(pageSize);
            }
            else
            {
                temp = temp.OrderByDescending<T, s>(orderbyLambda).Skip<T>((pageIndex - 1) * pageSize).Take<T>(pageSize);
            }
            return temp;
        }

        public bool Update(T model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            _context.Entry<T>(model).State = EntityState.Modified;
            return true;
        }
    }
}
