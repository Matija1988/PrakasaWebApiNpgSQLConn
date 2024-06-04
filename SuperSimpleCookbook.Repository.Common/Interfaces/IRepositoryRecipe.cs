using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSimpleCookbook.Repository.Common.Interfaces
{
    public interface IRepsitoryRecipe<T> where T : class
    {
        List<T> GetAll();
        T Get(int id);

        List<T> GetNotActive();

        T Post(T item);

        T Put(T item, int id);

        bool Delete(int id);


    }
}
