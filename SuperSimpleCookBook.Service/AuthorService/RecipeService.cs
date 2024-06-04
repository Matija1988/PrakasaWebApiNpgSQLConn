﻿using SuperSimpleCookbook.Model;
using SuperSimpleCookbook.Model.Model;
using SuperSimpleCookbook.Repository.Common.Interfaces;
using SuperSimpleCookbook.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SuperSimpleCookbook.Service.AuthorService
{
    public class RecipeService : IRecipeService<Recipe>
    {
        private readonly IRepositoryRecipe<Recipe> _repository;

        public RecipeService(IRepositoryRecipe<Recipe> repository)
        {
            _repository = repository;
        }

        public Task<Recipe> Create(Recipe entity)
        {
            return _repository.Post(entity);
        }

        public Task<bool> Delete(int id)
        {
            return _repository.Delete(id);
        }

        public async Task <ServiceResponse<List<Recipe>>> GetAll()
        {
            var response = await _repository.GetAll();

            if(response.Success == false)
            {
                response.Message = "No data in Database";
                response.Success = false;
            }

           return response;
        }

        public Task<Recipe> GetById(int id)
        {
            return _repository.Get(id); 
        }

        public Task<List<Recipe>> GetNotActive()
        {
            return _repository.GetNotActive();
        }

        public Task<Recipe> Update(Recipe entity, int id)
        {
            return _repository.Put(entity, id);
        }
    }
}
