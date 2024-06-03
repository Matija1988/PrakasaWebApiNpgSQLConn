using Microsoft.AspNetCore.Mvc;
using Npgsql;
using SuperSimpleCookbook.Model;

namespace SuperSimpleCookbook.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecipeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly NpgsqlConnection _connection;
        public RecipeController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            string commandText = "SELECT * FROM \"Recipe\" where \"IsActive\" = true;";
            var listFromDB = new List<Recipe>();
            var command = new NpgsqlCommand(commandText, _connection);

            _connection.Open();

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                listFromDB.Add(new Recipe
                {

                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Subtitle = reader.GetString(2),
                    Text = reader.GetString(3),
                    IsActive = reader.GetBoolean(4),
                    DateCreated = reader.GetDateTime(5),
                    DateUpdated = reader.GetDateTime(6),

                });
            }
            if (listFromDB is null)
            {
                return NotFound("No data in database!");
            }

            _connection.Close();
            await reader.DisposeAsync();

            return Ok(listFromDB);
        }
    }
}
