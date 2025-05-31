using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sigma.Domain.Interfaces.Infra.Data
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T entity);
        //Task UpdateAsync(T entity);
        //Task DeleteAsync(T entity);
        //Task<T> GetAsync(int id);
        //IQueryable<T> GetAll();
    }
}
