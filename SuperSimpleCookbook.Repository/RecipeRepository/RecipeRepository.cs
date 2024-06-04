using SuperSimpleCookbook.Model;
using SuperSimpleCookbook.Repository.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSimpleCookbook.Repository.RecipeRepository
{
    public class RecipeRepository : IRepositoryRecipe<Recipe>
    {
        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Recipe> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Recipe>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<List<Recipe>> GetNotActive()
        {
            throw new NotImplementedException();
        }

        public Task<Recipe> Post(Recipe item)
        {
            throw new NotImplementedException();
        }

        public Task<Recipe> Put(Recipe item, int id)
        {
            throw new NotImplementedException();
        }
    }
}
