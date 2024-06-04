using Npgsql;

using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using SuperSimpleCookbook.Model;
using SuperSimpleCookbook.Service.Common;

namespace SuperSimpleCookbook.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly NpgsqlConnection _connection;
        private readonly IAuthorService<Author> _service;
        public AuthorController(IAuthorService<Author> service)
        {
            _service = service;

            //_configuration = configuration;
            //_connection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var listFromDB = await _service.GetAll();

            if (listFromDB == null) 
            {
                return NotFound();
            }

            return Ok(listFromDB);
        }

        [HttpGet]
        [Route("NotActive")]
        public async Task<IActionResult> GetAllNotActive()
        {
            var listFromDb = await _service.GetNotActive();

            if (listFromDb == null)
            {
                return NotFound();
            }
            return Ok(listFromDb);
        }


        [HttpGet]
        [Route("{uuid:guid}")]
        public async Task<IActionResult> GetByGuid(Guid Uuid)
        {
            var entityFromDb = await _service.GetByGuid(Uuid);

            if (entityFromDb is null)
            {
                return NotFound();
            }

            return Ok(entityFromDb);    

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
