using SuperSimpleCookbook.Model.Model;

namespace SuperSimpleCookbook.Repository.Common.Interfaces
{
    public interface IRepositoryRecipe<T> where T : class
    {
        Task <ServiceResponse<List<T>>> GetAll();
        Task <ServiceResponse<T>> Get(int id);

        Task <ServiceResponse<List<T>>> GetNotActive();

        Task <ServiceResponse<T>> Post(T item);

        Task <ServiceResponse<T>> Put(T item, int id);

        Task<bool> Delete(int id);


    }
}
