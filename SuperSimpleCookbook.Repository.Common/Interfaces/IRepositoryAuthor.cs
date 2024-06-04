using SuperSimpleCookbook.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSimpleCookbook.Repository.Common.Interfaces
{
    public interface IRepositoryAuthor<T , T2> where T : class
    {
        Task <ServiceResponse<List<T>>> GetAll();
        Task <ServiceResponse<T>> Get(Guid uuid);

         Task <ServiceResponse<List<T>>> GetNotActive();

        Task <ServiceResponse<T>> Post(T item);

        Task <ServiceResponse<T>> Put(T item, Guid uuid);

        Task<bool> Delete(Guid uuid);

        Task<List<T2>> GetRecepiesByAuthorGuid(Guid uuid);
    }
}
