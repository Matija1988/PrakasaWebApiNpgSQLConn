using SuperSimpleCookbook.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSimpleCookbook.Service.Common
{
    public interface IAuthorService<T, T2> where T : class
    {
        Task <ServiceResponse<List<T>>> GetAll();

        Task <ServiceResponse<T>> GetByGuid(Guid uuid);

        Task <ServiceResponse<List<T>>> GetNotActive();

        Task <ServiceResponse<T>> Create(T entity);

        Task <ServiceResponse<T>> Update(T entity, Guid uuid);

        Task<bool> Delete(Guid uuid);

        Task <ServiceResponse<List<T2>>> GetRecepiesByAuthorGuid(Guid uuid);
    }
}
