using SuperSimpleCookbook.Common;
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

namespace SuperSimpleCookbook.Service
{
   
    public class RecipeService : IRecipeService<Recipe>
    {
        #region Constructor
        private readonly IRepositoryRecipe<Recipe> _repository;

        public RecipeService(IRepositoryRecipe<Recipe> repository)
        {
            _repository = repository;
        }

        #endregion
        public async Task<ServiceResponse<Recipe>> CreateAsync(Recipe entity)
        {
            entity.DateCreated = DateTime.Now;  
            entity.DateUpdated = DateTime.Now;
            entity.IsActive = true;

            var response = await _repository.PostAsync(entity);

            if (response.Success == false)
            {
                return response;
            }

            return response;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        #region GetMethods

        public async Task<ServiceResponse<List<Recipe>>> GetAllAsync()
        {
            var response = await _repository.GetAllAsync();

            if (response.Success == false)
            {
                response.Message = "No data in Database";
                response.Success = false;
            }

            return response;
        }

        public async Task<ServiceResponse<Recipe>> GetByIdAsync(int id)
        {
            var response = await _repository.GetAsync(id);

            if (response.Success == false)
            {
                return response;
            }

            return response;
        }

        public async Task<ServiceResponse<List<Recipe>>> GetNotActiveAsync()
        {
            var response = await _repository.GetNotActiveAsync();

            if (response.Success == false)
            {
                return response;
            }

            return response;
        }

        public async Task<ServiceResponse<List<Recipe>>>
            GetRecipeWithPFSAsync(FilterForRecipe filter, Paging paging, SortOrder sort)
        {
            var response = await _repository.GetRecipeWithFilterPagingAndSortAsync(filter, paging, sort);

            if (response.Success == false)
            {
                return response;

            }

            return response;
        }

        #endregion

        public async Task<ServiceResponse<Recipe>> UpdateAsync(Recipe entity, int id)
        {            
           
            entity.DateUpdated = DateTime.UtcNow;
           
            var response = await _repository.PutAsync(entity, id);

            if (response.Success == false)
            {
                return response;
            }

            return response;
        }
    }
}
