using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using SuperSimpleCookbook.Common;
using SuperSimpleCookbook.Model;
using SuperSimpleCookbook.Service.Common;

namespace SuperSimpleCookbook.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecipeController : ControllerBase
    {


        private readonly IRecipeService<Recipe> _service;

        private readonly IMapper _mapper;
        public RecipeController(IRecipeService<Recipe> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        #region GetMethods

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _service.GetAllAsync();

            if (response.Success == false)
            {
                return NotFound(response.Message);
            }

            List<RecipeReadDTO> recipeDTOs = new List<RecipeReadDTO>();

            foreach (var item in response.Data) 
            {
                recipeDTOs.Add(_mapper.Map<Recipe, RecipeReadDTO>(item));
            }

            return Ok(recipeDTOs);

        }
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetbyId(int id)
        {
            var response = await _service.GetByIdAsync(id);

            if (response.Success == false)
            {
                return NotFound(response.Message);
            }

            var recipeDTO = new RecipeReadDTO();
            recipeDTO = _mapper.Map<Recipe, RecipeReadDTO>(response.Data);

            return Ok(recipeDTO);

        }

        [HttpGet]
        [Route("NotActive")]
        public async Task<IActionResult> GetNotActive()
        {
            var response = await _service.GetNotActiveAsync();

            if (response.Success == false)
            {
                return NotFound(response.Message);
            }

            List<RecipeReadDTO> recipeDTOs = new List<RecipeReadDTO>();

            foreach (var item in response.Data)
            {
                recipeDTOs.Add(_mapper.Map<Recipe, RecipeReadDTO>(item));
            }

            return Ok(recipeDTOs);
        }

        #endregion

        [HttpPost]
        [Route("CreateRecipe")]

        public async Task<IActionResult> Post([FromBody] RecipeCreateDTO item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var newRecipe = _mapper.Map<RecipeCreateDTO, Recipe>(item);

            var response = await _service.CreateAsync(newRecipe);

            if (response.Success == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response.Message);
            }

            return StatusCode(StatusCodes.Status201Created, response.Data);

        }

        [HttpPut]
        [Route("UpdateRecipe/{id:int}")]
        public async Task<IActionResult> Put([FromBody] Recipe newRecipe, int id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _service.UpdateAsync(newRecipe, id);

            if (response.Success == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response.Message);
            }

            return Ok(response.Data);

        }

        [HttpDelete]
        [Route("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _service.DeleteAsync(id);

            if(response)
            {
                return Ok();    
            }

            return NotFound();
        }

        [HttpGet]
        [Route("paginate")]
        public async Task<IActionResult> AllWithPaginationFilteringAndSorting(
            [FromQuery] FilterForRecipe filter,
            [FromQuery] Paging paging,
            [FromQuery] SortOrder sort)
        {
            var response = await _service.GetRecipeWithFilterPagingAndSortAsync(filter, paging, sort);

            if (response.Success == false) 
            { 
                return BadRequest(response.Message);
            }

            List<RecipeReadDTO> recipeDTOs = new List<RecipeReadDTO>();

            foreach (var item in response.Data) 
            {
                recipeDTOs.Add(_mapper.Map<Recipe, RecipeReadDTO>(item));
            }

            return Ok(recipeDTOs);
        }

    }
}
