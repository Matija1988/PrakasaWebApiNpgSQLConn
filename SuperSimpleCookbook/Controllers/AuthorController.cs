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

            await  _connection.OpenAsync();

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                listFromDB.Add(new Author
                {

                    Id = reader.GetInt32(0),
                    Uuid = reader.GetGuid(1),
                    FirstName = reader.GetString(2),
                    LastName = reader.GetString(3),
                    DateOfBirth = reader.GetDateTime(4),
                    Bio = reader.GetString(5),
                    IsActive = reader.GetBoolean(6),
                    DateCreated = reader.GetDateTime(7),
                    DateUpdated = reader.GetDateTime(8),

                });
            }
            if (listFromDB is null)
            {
                return NotFound("No data in database!");
            }

            await  _connection.CloseAsync();
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

            await _connection.OpenAsync();

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                listFromDB.Add(new Author
                {

                    Id = reader.GetInt32(0),
                    Uuid = reader.GetGuid(1),
                    FirstName = reader.GetString(2),
                    LastName = reader.GetString(3),
                    DateOfBirth = reader.GetDateTime(4),
                    Bio = reader.GetString(5),
                    IsActive = reader.GetBoolean(6),
                    DateCreated = reader.GetDateTime(7),
                    DateUpdated = reader.GetDateTime(8)


                });
            }
            if (listFromDB is null)
            {
                return NotFound("No data in database!");
            }

            await _connection.CloseAsync();
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

            await _connection.OpenAsync();

            var reader = command.ExecuteReader();

            var author = new Author
            {
                Id = reader.GetInt32(0),
                Uuid = reader.GetGuid(1),
                FirstName = reader.GetString(2),
                LastName = reader.GetString(3),
                DateOfBirth = reader.GetDateTime(4),
                Bio = reader.GetString(5),
                IsActive = reader.GetBoolean(6),
                DateCreated = reader.GetDateTime(7),
                DateUpdated = reader.GetDateTime(8)
            };


            if (author is null)
            {
                return NotFound();
            }

            await _connection.CloseAsync();
            await _connection.DisposeAsync();

            return Ok(author);

        }

        [HttpPost]
        [Route("CreateAuthor")]

        public async Task<IActionResult> Create([FromBody] Author author)
        {
            try
            {
                string commandText = "INSERT INTO \"Author\" (\"Uuid\", \"FirstName\", \"LastName\",\"DateOfBirth\", \"Bio\", \"IsActive\", \"DateCreated\", \"DateUpdated\") "
                    + " VALUES (@Uuid, @FirstName, @LastName, @DateOfBirth, @Bio, @IsActive, @DateCreated, @DateUpdated) RETURNING \"Id\"";


                using var cmd = _connection.CreateCommand();
                cmd.CommandText = commandText;
                AddParameters(cmd, author);
                await _connection.OpenAsync();
                var rowAffected = await cmd.ExecuteNonQueryAsync();
                await _connection.CloseAsync();
                await _connection.DisposeAsync();
                return StatusCode(StatusCodes.Status201Created, author);
            }
            catch (Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        [HttpPut]
        [Route("UpdateAuthor/{id:int}")]
        public async Task<IActionResult> Update([FromBody]Author author, int id)
        {
            try { 
            const string commandText = "UPDATE \"Author\" SET \"Id\" = @Id, \"Uuid\" =@Uuid, \"FirstName\" = @FirstName, " +
                "\"LastName\" = @LastName, \"DateOfBirth\" = @DateOfBirth, \"Bio\" = @Bio, \"IsActive\" = @IsActive, " +
       "\"DateUpdated\" = @DateUpdated WHERE \"Id\" = @Id;";

            using var cmd = _connection.CreateCommand();
            cmd.CommandText = commandText;
            AddParameters(cmd, author);

            int tempId = id;
            author.Id = tempId;
            cmd.Parameters.AddWithValue("@Id", author.Id);

            await _connection.OpenAsync();
            var rowAffected = await cmd.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
            await _connection.DisposeAsync();
            return StatusCode(StatusCodes.Status200OK, author);
            }
            catch(Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteAuthor/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try { 
            const string commandText = "DELETE FROM \"Author\" WHERE \"Id\" = @Id";
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = commandText;
            cmd.Parameters.AddWithValue("@Id", id);
            await _connection.OpenAsync();
            var rowAffected = await cmd.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
            return StatusCode(StatusCodes.Status200OK); 
            } 
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        private void AddParameters(NpgsqlCommand command, Author author)
        {
            Guid guid = Guid.NewGuid();
            
            command.Parameters.AddWithValue("@Uuid", author.Uuid = guid);
            command.Parameters.AddWithValue("@FirstName", author.FirstName);
            command.Parameters.AddWithValue("@LastName", author.LastName);
            command.Parameters.AddWithValue("@DateOfBirth", author.DateOfBirth);
            command.Parameters.AddWithValue("@Bio", author.Bio);
            command.Parameters.AddWithValue("@IsActive", author.IsActive);
            command.Parameters.AddWithValue("@DateCreated", author.DateCreated);
            command.Parameters.AddWithValue("@DateUpdated", author.DateUpdated);

        }


    }

}
