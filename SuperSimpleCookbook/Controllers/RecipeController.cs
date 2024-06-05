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
        public RecipeController(IRecipeService<Recipe> service)
        {
            _service = service;
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
        [Route("{id:int}")]
        public async Task<IActionResult> GetbyId(int id)
        {
            var response = await _service.GetByIdAsync(id);

            if (response.Success == false)
            {
                return NotFound(response.Message);
            }

            return Ok(response.Data);

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
            return Ok(response.Data);
        }

        [HttpPost]
        [Route("CreateRecipe")]

        public async Task<IActionResult> Post([FromBody] Recipe newRecipe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

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

            return Ok(response.Data);
        }

    }
}
