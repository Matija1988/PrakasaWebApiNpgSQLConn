using SuperSimpleCookbook.Common;
using SuperSimpleCookbook.Model;
using SuperSimpleCookbook.Model.Model;
using SuperSimpleCookbook.Repository.Common.Interfaces;
using SuperSimpleCookbook.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSimpleCookbook.Service.AuthorService
{
    public class AuthorService : IAuthorService<Author, AuthorRecipe>
    {
        private readonly IRepositoryAuthor<Author, AuthorRecipe> _repository; 
        public AuthorService(IRepositoryAuthor<Author, AuthorRecipe> repository) 
        {
            _repository = repository;
        }

        public async Task <ServiceResponse<Author>> CreateAsync(Author entity)
        {
            var response = await _repository.PostAsync(entity);

            if(response.Success == false)
            {
                return response;
            }

            return response;
        }

        public Task<bool> Delete(Guid uuid)
        {
            return _repository.DeleteAsync(uuid);
        }

        public Task<bool> DeleteAsync(Guid uuid)
        {
            throw new NotImplementedException();
        }

        public async Task <ServiceResponse<List<Author>>> GetAllAsync()
        {
            var response = await _repository.GetAllAsync();

            if(response.Success == false)
            {
                return response;
            }
            return response;
            
        }

      
        public async Task <ServiceResponse<Author>> GetByGuidAsync(Guid uuid)
        {
            var response = await _repository.GetAsync(uuid);

            if(response.Success == false)
            {
                return response;
            }

            return response;
        }

        public async Task <ServiceResponse<List<Author>>> GetNotActiveAsync()
        {
            var response = await _repository.GetNotActiveAsync();

            if(response.Success == false)
            {
                return response;
            }

            return response;
        }

        public async Task <ServiceResponse<List<AuthorRecipe>>> GetRecepiesByAuthorGuidAsync(Guid uuid)
        {
            var response = await _repository.GetRecepiesByAuthorGuidAsync(uuid);

            if(response.Success == false)
            {
                return response;
            }

            return response;
        }

        public async Task <ServiceResponse<Author>> UpdateAsync(Author entity, Guid uuid)
        {
            var response = await _repository.PutAsync(entity,uuid);

            if (response.Success == false) 
            {
                return response;
            }
            return response;

        }

        public async Task<ServiceResponse<List<Author>>> GetAuthorWithFilterPagingAndSortAsync
            (FilterForAuthor filter, Paging paging, SortOrder sort)
        {   
         var response = await _repository.GetAuthorWithFilterPagingAndSortAsync(filter, paging, sort);

            if(response.Success == false)
            {
                return response;
            }

                return response;

        }



    }
}
