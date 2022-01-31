using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtoVen.API.Repository
{
    public interface IGenericRepository<T> where T: class
    {
        Task<T> GetById(int Id);
        Task<IEnumerable<T>> GetAll(int Id);
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(T entity);
    }
}
