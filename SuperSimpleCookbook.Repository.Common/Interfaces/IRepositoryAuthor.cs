using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSimpleCookbook.Repository.Common.Interfaces
{
    public interface IRepositoryAuthor<T> where T : class
    {
        Task<List<T>> GetAll();
        Task<T> Get(Guid uuid);

         Task<List<T>> GetNotActive();

        Task<T>Post(T item);

        Task<T> Put(T item, Guid uuid);

        Task<bool> Delete(Guid uuid);
    }
}
