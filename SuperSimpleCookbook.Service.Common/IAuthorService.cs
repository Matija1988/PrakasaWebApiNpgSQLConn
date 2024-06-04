using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSimpleCookbook.Service.Common
{
    public interface IAuthorService<T> where T : class
    {
        Task<List<T>> GetAll();

        Task<T> GetByGuid(Guid uuid);

        Task<List<T>> GetNotActive();
    }
}
