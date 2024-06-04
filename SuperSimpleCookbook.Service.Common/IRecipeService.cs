using SuperSimpleCookbook.Model.Model;

namespace SuperSimpleCookbook.Service.Common
{
    public interface IRecipeService<T> where T : class
    {
         Task <ServiceResponse<List<T>>> GetAll();

        Task <ServiceResponse<T>> GetById(int id);

        Task <ServiceResponse<List<T>>> GetNotActive();

        Task<T> Create(T entity);

        Task<T> Update(T entity, int id);

        Task<bool> Delete(int id);
    }
}
