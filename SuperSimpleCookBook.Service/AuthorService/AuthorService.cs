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

        public async Task <ServiceResponse<Author>> Create(Author entity)
        {
            var response = await _repository.Post(entity);

            if(response.Success == false)
            {
                return response;
            }

            return response;
        }

        public Task<bool> Delete(Guid uuid)
        {
            return _repository.Delete(uuid);
        }

        public async Task <ServiceResponse<List<Author>>> GetAll()
        {
            var response = await _repository.GetAll();

            if(response.Success == false)
            {
                return response;
            }
            return response;
            
        }

        public async Task <ServiceResponse<Author>> GetByGuid(Guid uuid)
        {
            var response = await _repository.Get(uuid);

            if(response.Success == false)
            {
                return response;
            }

            return response;
        }

        public async Task <ServiceResponse<List<Author>>> GetNotActive()
        {
            var response = await _repository.GetNotActive();

            if(response.Success == false)
            {
                return response;
            }

            return response;
        }

        public async Task<List<AuthorRecipe>> GetRecepiesByAuthorGuid(Guid uuid)
        {
            return await _repository.GetRecepiesByAuthorGuid(uuid);
        }

        public async Task<Author> Update(Author entity, Guid uuid)
        {
            return await _repository.Put(entity,uuid);
        }


    }
}
