using BCrypt.Net;
using SuperSimpleCookbook.Common;
using SuperSimpleCookbook.Model;
using SuperSimpleCookbook.Model.Model;
using SuperSimpleCookbook.Repository.Common.Interfaces;
using SuperSimpleCookbook.Service.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SuperSimpleCookbook.Service
{

    #region Constructor
    public class AuthorService : IAuthorService<Author, AuthorRecipe>
    {
        private readonly IRepositoryAuthor<Author, AuthorRecipe> _repository;
        public AuthorService(IRepositoryAuthor<Author, AuthorRecipe> repository)
        {
            _repository = repository;
        }

        #endregion



        public async Task<ServiceResponse<Author>> CreateAsync(Author entity)
        {
            var response = new ServiceResponse<Author>();
            entity.IsActive = true;
            entity.DateCreated = DateTime.Now;
            entity.DateUpdated = DateTime.Now;
            entity.Uuid = Guid.NewGuid();


           
            if (await ValidateUsername(entity) == false)
            {
                response.Success = false;
                response.Message = "Username in use!";
                return response;
            }

            string password = BCrypt.Net.BCrypt.HashPassword(entity.Password);

            entity.Password = password;

            response = await _repository.PostAsync(entity);


            if (response.Success == false)
            {
                return response;
            }

            return response;
        }

        public async Task<bool> DeleteAsync(Guid uuid)
        {
            return await _repository.DeleteAsync(uuid);
            
        }

        #region GetMethods
        public async Task<ServiceResponse<List<Author>>> GetAllAsync()
        {
            var response = await _repository.GetAllAsync();

            if (response.Success == false)
            {
                return response;
            }
            return response;

        }


        public async Task<ServiceResponse<Author>> GetByGuidAsync(Guid uuid)
        {
            var response = await _repository.GetAsync(uuid);

            Debug.WriteLine(response.Items);

            if (response.Success == false)
            {
                return response;
            }

            return response;
        }

        public async Task<ServiceResponse<List<Author>>> GetNotActiveAsync()
        {
            var response = await _repository.GetNotActiveAsync();

            if (response.Success == false)
            {
                return response;
            }

            return response;
        }

        public async Task<ServiceResponse<List<AuthorRecipe>>> GetRecepiesByAuthorGuidAsync(Guid uuid)
        {
            var response = await _repository.GetRecepiesByAuthorGuidAsync(uuid);

            if (response.Success == false)
            {
                return response;
            }

            return response;
        }


        public async Task<ServiceResponse<List<Author>>> GetAuthorWithPFSAsync
            (FilterForAuthor filter, Paging paging, SortOrder sort)
        {
            var response = await _repository.GetAuthorWithFilterPagingAndSortAsync(filter, paging, sort);

            if (response.Success == false)
            {
                return response;
            }

            response.PageCount = (int)Math.Ceiling(response.TotalCount / (double)paging.PageSize);
                
            
            
            return response;

        }

        #endregion

        public async Task<ServiceResponse<Author>> UpdateAsync(Author entity, Guid uuid)
        {
            entity.DateUpdated = DateTime.Now;
            entity.IsActive = true;

            var response = await _repository.PutAsync(entity, uuid);

            if (response.Success == false)
            {
                return response;
            }
            return response;

        }

        private async Task<bool> ValidateUsername(Author entity)
        {
            var userNameValidation = await _repository.GetAllAsync();

            foreach (var userName in userNameValidation.Items)
            {
                if (userName.Username == entity.Username)
                {
                    return false;
                }
            }

            return true;
        }



    }
}
