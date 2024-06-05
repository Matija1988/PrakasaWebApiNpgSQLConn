using Npgsql;

using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using SuperSimpleCookbook.Model;
using SuperSimpleCookbook.Service.Common;
using SuperSimpleCookbook.Model.Model;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using SuperSimpleCookbook.Common; 

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
        [Route("paginate")]
        public async Task<IActionResult> GetAllWithPaginationFilteringAndSorting(
            [FromQuery]FilterForAuthor filter, 
            [FromQuery]Paging paging, 
            [FromQuery]SortOrder sort)
        {
            var response =  await _service.GetAuthorWithFilterPagingAndSortAsync(filter, paging, sort);

            if(response.Success == false)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.Data);

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _service.GetAllAsync();

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
            var response = await _service.GetNotActiveAsync();

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
            var response = await _service.GetByGuidAsync(Uuid);

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

            var response = await _service.CreateAsync(author);

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
            var response = await _service.UpdateAsync(author, uuid);

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
           var response = await _service.DeleteAsync(uuid);

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
            var response = await _service.GetRecepiesByAuthorGuidAsync(uuid);

            if(response.Success == false)
            {
                return NotFound(response.Message);
            }
            return Ok(response.Data);

        }


    }

}
