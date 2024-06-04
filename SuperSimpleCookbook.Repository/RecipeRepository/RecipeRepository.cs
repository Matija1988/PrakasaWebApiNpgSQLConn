using SuperSimpleCookbook.Model;
using SuperSimpleCookbook.Repository.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSimpleCookbook.Repository.RecipeRepository
{
    public class RecipeRepository : IRepsitoryRecipe<Recipe>
    {
        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Recipe Get(int id)
        {
            throw new NotImplementedException();
        }

        public List<Recipe> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<Recipe> GetNotActive()
        {
            throw new NotImplementedException();
        }

        public Recipe Post(Recipe item)
        {
            throw new NotImplementedException();
        }

        public Recipe Put(Recipe item, int id)
        {
            throw new NotImplementedException();
        }
    }
}
