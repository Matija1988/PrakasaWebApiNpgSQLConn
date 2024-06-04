using SuperSimpleCookbook.Model;
using SuperSimpleCookbook.Model.Model;
using SuperSimpleCookbook.Repository.Common.Interfaces;
using SuperSimpleCookbook.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SuperSimpleCookbook.Service.RecipeService
{
    public class RecipeService : IRecipeService<Recipe>
    {
        private readonly IRepositoryRecipe<Recipe> _repository;

        public RecipeService(IRepositoryRecipe<Recipe> repository)
        {
            _repository = repository;
        }

        public async Task <ServiceResponse<Recipe>> Create(Recipe entity)
        {
            var response = await _repository.Post(entity);

            if (response.Success == false) 
            {
                return response;
            }

            return response;
        }

        public Task<bool> Delete(int id)
        {
            return _repository.Delete(id);
        }

        public async Task<ServiceResponse<List<Recipe>>> GetAll()
        {
            var response = await _repository.GetAll();

            if (response.Success == false)
            {
                response.Message = "No data in Database";
                response.Success = false;
            }

            return response;
        }

        public async Task<ServiceResponse<Recipe>> GetById(int id)
        {
            var response = await _repository.Get(id);

            if (response.Success == false)
            {
                return response;
            }

            return response;
        }

        public async Task <ServiceResponse<List<Recipe>>> GetNotActive()
        {
            var response = await _repository.GetNotActive();

            if (response.Success == false) 
            {
                return response;
            }

            return response;
        }

        public async Task <ServiceResponse<Recipe>> Update(Recipe entity, int id)
        {
            var response = await _repository.Put(entity, id);

            if (response.Success == false)
            {
                return response;
            }

            return response;
        }
    }
}
