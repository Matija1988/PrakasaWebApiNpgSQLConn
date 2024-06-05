using SuperSimpleCookbook.Common;
using SuperSimpleCookbook.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace SuperSimpleCookbook.Service.Common
{
    public interface IAuthorService<T, T2> where T : class
    {
        Task <ServiceResponse<List<T>>> GetAllAsync();

        Task <ServiceResponse<T>> GetByGuidAsync(Guid uuid);

        Task <ServiceResponse<List<T>>> GetNotActiveAsync();

        Task <ServiceResponse<T>> CreateAsync(T entity);

        Task <ServiceResponse<T>> UpdateAsync(T entity, Guid uuid);

        Task<bool> DeleteAsync(Guid uuid);

        Task <ServiceResponse<List<T2>>> GetRecepiesByAuthorGuidAsync(Guid uuid);
        Task<ServiceResponse<List<T>>>
            GetAuthorWithFilterPageingAndSort(FilterForAuthor filter, Paging paging, SortOrder sort);
    }
}
