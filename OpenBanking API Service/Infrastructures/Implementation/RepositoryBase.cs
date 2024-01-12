using Microsoft.EntityFrameworkCore;
using OpenBanking_API_Service.Data;
using OpenBanking_API_Service.Infrastructures.Interface;
using System.Linq.Expressions;

namespace OpenBanking_API_Service.Infrastructures.Implementation
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        public ApplicationDbContext _applicationDbContext;
        public RepositoryBase(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public void Create(T entity)
        {
            _applicationDbContext.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            _applicationDbContext.Set<T>().Remove(entity);
        }

        public IQueryable<T> FindAll(bool trackChanges)
        {
            return !trackChanges ?
                    _applicationDbContext.Set<T>()
                    .AsNoTracking() :
                    _applicationDbContext.Set<T>();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges)
        {
            return !trackChanges ?
                    _applicationDbContext.Set<T>()
                    .Where(expression)
                    .AsNoTracking() :
                    _applicationDbContext.Set<T>()
                        .Where(expression);
        }

        public void Update(T entity)
        {
            _applicationDbContext.Set<T>().Update(entity);
        }
    }
}
