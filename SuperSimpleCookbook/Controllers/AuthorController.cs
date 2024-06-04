using Npgsql;

using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using SuperSimpleCookbook.Model;
using SuperSimpleCookbook.Service.Common;

namespace SuperSimpleCookbook.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly NpgsqlConnection _connection;
        private readonly IAuthorService<Author> _service;
        public AuthorController(IAuthorService<Author> service)
        {
            _service = service;

            //_configuration = configuration;
            //_connection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var listFromDB = await _service.GetAll();

            if (listFromDB == null) 
            {
                return NotFound();
            }

            return Ok(listFromDB);
        }

        [HttpGet]
        [Route("NotActive")]
        public async Task<IActionResult> GetAllNotActive()
        {
            var listFromDb = await _service.GetNotActive();

            if (listFromDb == null)
            {
                return NotFound();
            }
            return Ok(listFromDb);
        }


        [HttpGet]
        [Route("{uuid:guid}")]
        public async Task<IActionResult> GetByGuid(Guid Uuid)
        {
            var entityFromDb = await _service.GetByGuid(Uuid);

            if (entityFromDb is null)
            {
                return NotFound();
            }

            return Ok(entityFromDb);    

        }

        [HttpPost]
        [Route("CreateAuthor")]

        public async Task<IActionResult> Create([FromBody] Author author)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }


            var response = await _service.Create(author);

            return StatusCode(StatusCodes.Status201Created, response);

        }

        [HttpPut]
        [Route("UpdateAuthor/{uuid:Guid}")]
        public async Task<IActionResult> Update([FromBody]Author author, Guid uuid)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest();
            }
            var response = await _service.Update(author, uuid);

            return Ok(response);

        }

        [HttpDelete]
        [Route("DeleteAuthor/{uuid:Guid}")]
        public async Task<IActionResult> Delete(Guid uuid)
        {
           var response = await _service.Delete(uuid);

            if(response)
            {
                return Ok();
            }
            return NotFound();
        }

      


    }

}
