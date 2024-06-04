using SuperSimpleCookbook.Model;
using SuperSimpleCookbook.Repository.Common.Interfaces;
using SuperSimpleCookbook.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSimpleCookbook.Service.AuthorService
{
    public class AuthorService : IAuthorService<Author>
    {
        private readonly IRepositoryAuthor<Author> _repository; 
        public AuthorService(IRepositoryAuthor<Author> repository) 
        {
            _repository = repository;
        }

        public async Task<List<Author>> GetAll()
        {
         return await _repository.GetAll();
            
        }

        public async Task<Author> GetByGuid(Guid uuid)
        {
            return await _repository.Get(uuid);
        }

        public async Task<List<Author>> GetNotActive()
        {
           return await _repository.GetNotActive();
        }
    }
}
