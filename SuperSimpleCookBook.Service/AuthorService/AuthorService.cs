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

        public async Task<Author> Create(Author entity)
        {
            return await _repository.Post(entity);
        }

        public Task<bool> Delete(Guid uuid)
        {
            return _repository.Delete(uuid);
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

        public async Task<Author> Update(Author entity, Guid uuid)
        {
            return await _repository.Put(entity,uuid);
        }
    }
}
