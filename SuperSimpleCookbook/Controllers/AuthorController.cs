using Npgsql;

using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using SuperSimpleCookbook.Model;
using SuperSimpleCookbook.Service.Common;
using SuperSimpleCookbook.Model.Model;

namespace SuperSimpleCookbook.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorController : ControllerBase
    {
        
        private readonly IAuthorService<Author, AuthorRecipe> _service;
        public AuthorController(IAuthorService<Author, AuthorRecipe> service)
        {
            _service = service;

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _service.GetAll();

            if (response.Success == false) 
            {
                return NotFound(response.Message);
            }

            return Ok(response.Data);
        }

        [HttpGet]
        [Route("NotActive")]
        public async Task<IActionResult> GetAllNotActive()
        {
            var response = await _service.GetNotActive();

            if (response.Success == false)
            {
                return NotFound(response.Message);
            }
            return Ok(response.Data);
        }


        [HttpGet]
        [Route("{uuid:guid}")]
        public async Task<IActionResult> GetByGuid(Guid Uuid)
        {
            var response = await _service.GetByGuid(Uuid);

            if (response.Success == false)
            {
                return NotFound(response.Message);
            }

            return Ok(response.Data);    

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

            if (response.Success == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response.Message);
            }

            return StatusCode(StatusCodes.Status201Created, response.Data);

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

            if (response.Success == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response.Message);
            }

            return Ok(response.Data);

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

        [HttpGet]
        [Route("AuthorRecipe/{uuid:Guid}")]

        public async Task<IActionResult> GetAuthorRecipe(Guid uuid)
        {
            var response = await _service.GetRecepiesByAuthorGuid(uuid);

            if(response.Success == false)
            {
                return NotFound(response.Message);
            }
            return Ok(response.Data);

        }


    }

}
