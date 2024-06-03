using Npgsql;
using SuperSimpleCookbook.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;

namespace SuperSimpleCookbook.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly NpgsqlConnection _connection;
        public AuthorController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
           

            string commandText = "SELECT * FROM \"Author\" where \"IsActive\" = true;";
            var listFromDB = new List<Author>();
            var command = new NpgsqlCommand(commandText, _connection);

            _connection.Open();

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                listFromDB.Add(new Author
                {

                    Id = reader.GetInt32(0),
                    Uuid = reader.GetGuid(1),
                    FirstName = reader.GetString(2),
                    LastName = reader.GetString(3),
                    Bio = reader.GetString(5),

                });  
            }
            if(listFromDB is null)
            {
                return NotFound("No data in database!");
            }

            _connection.Close();
            await reader.DisposeAsync();

            return Ok(listFromDB);
        }

        [HttpGet]
        [Route("NotActive")]
        public async Task<IActionResult> GetAllNotActive()
        {


            string commandText = "SELECT * FROM \"Author\" where \"IsActive\" = false;";
            var listFromDB = new List<Author>();
            var command = new NpgsqlCommand(commandText, _connection);

            _connection.Open();

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                listFromDB.Add(new Author
                {

                    Id = reader.GetInt32(0),
                    Uuid = reader.GetGuid(1),
                    FirstName = reader.GetString(2),
                    LastName = reader.GetString(3),
                    Bio = reader.GetString(5),

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


        [HttpGet]
        [Route("{guid}")]
        public async Task<IActionResult> GetByGuid(Guid guid)
        {
            string commandText = "SELECT * FROM \"Author\" WHERE \"Uuid\" = @guid;";
            
            var command = new NpgsqlCommand(commandText, _connection);
            command.Parameters.AddWithValue("@guid", guid);
            
            _connection.Open();

            var reader = command.ExecuteReader();

            var author = new Author 
            { 
            Id = reader.GetInt32(0),
            Uuid= reader.GetGuid(1),
            FirstName = reader.GetString(2),
            LastName = reader.GetString(3),
            DateOfBirth = reader.IsDBNull(reader.GetOrdinal("DateOfBirth")) 
            ? (DateOnly?)null : DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("DateOfBirth"))),
            Bio = reader.GetString(5)
            };


            if (author is null)
            {
                return NotFound();
            }

            _connection.Close();
            await _connection.DisposeAsync();

            return Ok(author);

        }

        [HttpPost]
        [Route("CreateAuthor")]

        public async Task<IActionResult> Create(Author author)
        {
            string commandText = "Insert into \"Author\" values (@Uuid, @FirstName, @LastName, @DateOfBirth, @Bio, @IsActive, @DateCreated, @DateUpdated)";
            var authorFromDb = new Author();
            var command = new NpgsqlCommand(commandText, _connection);

            try
            {
                Guid guid = Guid.NewGuid();

                command.Parameters.AddWithValue("@Uuid", guid);
                command.Parameters.AddWithValue("@FirstName", author.FirstName);
                command.Parameters.AddWithValue("@LastName", author.LastName);
                command.Parameters.AddWithValue("@DateOfBirth", author.DateOfBirth ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Bio", author.Bio);
                command.Parameters.AddWithValue("@IsActive", author.IsActive);
                command.Parameters.AddWithValue("@DateCreated", author.DateCreated);
                command.Parameters.AddWithValue("@DateUpdated", author.DateUpdated);

                return Ok(author);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
                
        }
    }
}
