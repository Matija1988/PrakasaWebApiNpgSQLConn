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
                Uuid = reader.GetGuid(1),
                FirstName = reader.GetString(2),
                LastName = reader.GetString(3),
                DateOfBirth = reader.GetDateTime(4),
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

        public async Task<bool> Create([FromBody] Author author)
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
            return rowAffected > 0;

        }

       // [HttpPut]
       // [Route("UpdateAuthor/{id:int}")]
       // public async Task<bool> Update(Author author)
       // {
       //     const string commandText =
       //"UPDATE \"Author\" SET \"FirstName\" = @FirstName, \"LastName\" = @LastName, " + "job_title = @JobTitle, salary = @Salary, hire_date = @HireDate WHERE id = @Id";

       //     //using var cmd = connection.CreateCommand();
       //     //cmd.CommandText = updateQuery;
       //     //AddParameters(cmd, employee);
       //     //await connection.OpenAsync();
       //     /var rowAffected = await cmd.ExecuteNonQueryAsync();
       //     //await connection.CloseAsync();
       //     return rowAffected > 0;
       // }
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
