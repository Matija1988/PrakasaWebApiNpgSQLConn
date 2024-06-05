using SuperSimpleCookbook.Model.Model;

namespace SuperSimpleCookbook.Repository.Common.Interfaces
{
    public interface IRepositoryRecipe<T> where T : class
    {
        Task <ServiceResponse<List<T>>> GetAllAsync();
        Task <ServiceResponse<T>> GetAsync(int id);

        Task <ServiceResponse<List<T>>> GetNotActiveAsync();

        Task <ServiceResponse<T>> PostAsync(T item);

        Task <ServiceResponse<T>> PutAsync(T item, int id);

        Task<bool> DeleteAsync(int id);


    }
}
