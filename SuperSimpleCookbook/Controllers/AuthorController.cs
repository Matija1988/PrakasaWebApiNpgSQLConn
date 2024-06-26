        using Microsoft.AspNetCore.Mvc;
using SuperSimpleCookbook.Model;
using SuperSimpleCookbook.Service.Common;
using SuperSimpleCookbook.Model.Model;
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

        #region Get Methods

        [HttpGet]
        [Route("paginate")]
        public async Task<IActionResult> GetAllWithPFSAsync(
            [FromQuery]FilterForAuthor filter, 
            [FromQuery]Paging paging, 
            [FromQuery]SortOrder sort)
        {
            var response =  await _service.GetAuthorWithPFSAsync(filter, paging, sort);

            if(response.Success == false)
            {
                return BadRequest(response.Message);
            }

            List<AuthorReadDTO> authorDTOs = new List<AuthorReadDTO>();

            foreach (var item in response.Items)
            {
                authorDTOs.Add(_mapper.Map<Author, AuthorReadDTO>(item));
            }

            var finalResponse = new ServiceResponse<List<AuthorReadDTO>>(); 
            
            finalResponse.Items = authorDTOs;
            finalResponse.TotalCount = response.TotalCount;
            finalResponse.PageCount = response.PageCount;
            finalResponse.Message = response.Message;
            finalResponse.Success = response.Success;

            return Ok(finalResponse);

        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _service.GetAllAsync();

            if (response.Success == false) 
            {
                return NotFound(response.Message);
            }

            List<AuthorReadDTO> authorDTOs = new List<AuthorReadDTO>();

            foreach (var item in response.Items) 
            {
                authorDTOs.Add(_mapper.Map<Author, AuthorReadDTO>(item));
            }

            return Ok(authorDTOs);
        }

        [HttpGet]
        [Route("NotActive")]
        public async Task<IActionResult> GetAllNotActiveAsync()
        {
            var response = await _service.GetNotActiveAsync();

            if (response.Success == false)
            {
                return NotFound(response.Message);
            }

            List<AuthorReadDTO> authorDTOs = new List<AuthorReadDTO>();

            foreach (var item in response.Items)
            {
                authorDTOs.Add(_mapper.Map<Author, AuthorReadDTO>(item));
            }
            
            return Ok(authorDTOs);
        }


        [HttpGet]
        [Route("{uuid:Guid}")]
        public async Task<IActionResult> GetByGuidAsync(Guid uuid)
        {
            var response = await _service.GetByGuidAsync(uuid);

            if (response.Success == false)
            {
                return NotFound(response.Message);
            }

            var authorDTO = _mapper.Map<Author, AuthorReadDTO>(response.Items);

            return Ok(authorDTO);    

        }

        [HttpGet]
        [Route("AuthorRecipe/{uuid:Guid}")]

        public async Task<IActionResult> GetAuthorRecipeAsync(Guid uuid)
        {
            var response = await _service.GetRecepiesByAuthorGuidAsync(uuid);

            if (response.Success == false)
            {
                return NotFound(response.Message);
            }

            return Ok(response.Items);

        }


        #endregion

        [HttpPost]
        [Route("CreateAuthor")]

        public async Task<IActionResult> CreateAsync([FromBody] AuthorCreateDTO authorDTO)
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
        public async Task<IActionResult> UpdateAsync([FromBody]AuthorUpdateDTO authorDto, Guid uuid)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest();
            }

            var author = new Author();
            author = _mapper.Map<AuthorUpdateDTO, Author>(authorDto);

            var response = await _service.UpdateAsync(author, uuid);

            if (response.Success == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response.Message);
            }
            
            return Ok();

        }

        [HttpDelete]
        [Route("DeleteAuthor/{uuid:Guid}")]
        public async Task<IActionResult> DeleteAsync(Guid uuid)
        {
           var response = await _service.DeleteAsync(uuid);

            if(response)
            {
                return Ok();
            }
            return NotFound();
        }


    }

}
