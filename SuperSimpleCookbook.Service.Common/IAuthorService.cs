using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSimpleCookbook.Service.Common
{
    public interface IAuthorService<T, T2> where T : class
    {
        Task<List<T>> GetAll();

        Task<T> GetByGuid(Guid uuid);

        Task<List<T>> GetNotActive();

        Task<T> Create(T entity);

        Task<T> Update(T entity, Guid uuid);

        Task<bool> Delete(Guid uuid);

        Task<List<T2>> GetRecepiesByAuthorGuid(Guid uuid);
    }
}
