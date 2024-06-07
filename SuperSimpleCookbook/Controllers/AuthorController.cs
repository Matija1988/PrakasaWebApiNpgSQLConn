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
using AutoMapper;

namespace SuperSimpleCookbook.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorController : ControllerBase
    {
        
        private readonly IAuthorService<Author, AuthorRecipe> _service;

        private readonly IMapper _mapper;

        public AuthorController(IAuthorService<Author, AuthorRecipe> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;

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

            List<AuthorReadDTO> authorDTO = new List<AuthorReadDTO>();

            foreach (var item in response.Data)
            {
                authorDTO.Add(_mapper.Map<Author, AuthorReadDTO>(item));
            }


            return Ok(authorDTO);

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _service.GetAllAsync();

            if (response.Success == false) 
            {
                return NotFound(response.Message);
            }

            List<AuthorReadDTO> authorDTO = new List<AuthorReadDTO>();

            foreach (var item in response.Data) 
            {
                authorDTO.Add(_mapper.Map<Author, AuthorReadDTO>(item));
            }

            return Ok(authorDTO);
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

            List<AuthorReadDTO> authorDTO = new List<AuthorReadDTO>();

            foreach (var item in response.Data)
            {
                authorDTO.Add(_mapper.Map<Author, AuthorReadDTO>(item));
            }

            return Ok(authorDTO);
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

            var authorDTO = _mapper.Map<Author, AuthorReadDTO>(response.Data);

            return Ok(authorDTO);    

        }

        [HttpPost]
        [Route("CreateAuthor")]

        public async Task<IActionResult> Create([FromBody] AuthorCreateDTO authorDTO)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            var author = new Author();

            author = _mapper.Map<AuthorCreateDTO, Author>(authorDTO);
            
            var response = await _service.CreateAsync(author);

            if (response.Success == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response.Message);
            }

            return StatusCode(StatusCodes.Status201Created);

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
