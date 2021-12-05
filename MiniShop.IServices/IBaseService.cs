using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MiniShop.IServices
{
    public interface IBaseService<T> where T : class
    {
        bool Save();
        Task<bool> SaveAsync();
        IQueryable<T> Select(Expression<Func<T, bool>> whereLambda);
        IQueryable<T> SelectPage<s>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, s>> orderbyLambda, bool isAsc);
        bool Delete(T model);
        bool Update(T model);
        T Insert(T model);
    }
}
