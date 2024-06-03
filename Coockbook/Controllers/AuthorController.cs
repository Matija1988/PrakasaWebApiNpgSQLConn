using Microsoft.AspNetCore.Mvc;
using Npgsql;
using SuperSimpleCookbook.Model;

namespace SuperSimpleCookbook.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly NpgsqlConnection _connectionString;
        public AuthorController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task<IActionResult> GetAll()
        {
            string commandText = "SELECT * FROM " + " Author";
            var listFromDB = new List<Author>();
            var command = new NpgsqlCommand(commandText, _connectionString);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                listFromDB.Add(new Author
                {
                    Id = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Bio = reader.GetString(3),

                });  
            }
            if(listFromDB == null)
            {
                return NotFound("No data in database!");
            }
            return Ok(listFromDB);
        }
    }
}
