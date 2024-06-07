using SuperSimpleCookbook.Common;
using SuperSimpleCookbook.Model.Model;

namespace SuperSimpleCookbook.Service.Common
{
    public interface IRecipeService<T> where T : class
    {
         Task <ServiceResponse<List<T>>> GetAllAsync();

        Task <ServiceResponse<T>> GetByIdAsync(int id);

        Task <ServiceResponse<List<T>>> GetNotActiveAsync();

        Task <ServiceResponse<T>> CreateAsync(T entity);

        Task <ServiceResponse<T>> UpdateAsync(T entity, int id);

        Task<bool> DeleteAsync(int id);

        Task<ServiceResponse<List<T>>>
            GetRecipeWithPFSAsync(FilterForRecipe filter, Paging paging, SortOrder sort);

    }
}
