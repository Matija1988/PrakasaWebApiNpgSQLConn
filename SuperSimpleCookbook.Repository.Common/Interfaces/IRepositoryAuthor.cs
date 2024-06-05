using SuperSimpleCookbook.Common;
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
        Task <ServiceResponse<List<T>>> GetAllAsync();
        Task <ServiceResponse<T>> GetAsync(Guid uuid);

         Task <ServiceResponse<List<T>>> GetNotActiveAsync();

        Task <ServiceResponse<T>> PostAsync(T item);

        Task <ServiceResponse<T>> PutAsync(T item, Guid uuid);

        Task<bool> DeleteAsync(Guid uuid);

        Task <ServiceResponse<List<T2>>> GetRecepiesByAuthorGuidAsync(Guid uuid);

        Task<ServiceResponse<List<T>>> 
            GetAuthorWithFilterPageingAndSort(FilterForAuthor filter, Paging paging, SortOrder sort);
    }
}
