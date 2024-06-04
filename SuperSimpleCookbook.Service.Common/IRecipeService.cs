using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSimpleCookbook.Service.Common
{
    public interface IRecipeService<T> where T : class
    {
        Task<List<T>> GetAll();

        Task<T> GetById(int id);

        Task<List<T>> GetNotActive();

        Task<T> Create(T entity);

        Task<T> Update(T entity, int id);

        Task<bool> Delete(int id);
    }
}
