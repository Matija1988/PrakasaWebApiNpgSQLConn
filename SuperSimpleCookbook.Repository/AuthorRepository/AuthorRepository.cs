using Npgsql;
using SuperSimpleCookbook.Model;
using SuperSimpleCookbook.Model.Model;
using SuperSimpleCookbook.Repository.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace SuperSimpleCookbook.Repository.AuthorRepository
{
    public class AuthorRepository : IRepositoryAuthor<Author, AuthorRecipe>
    {
       
        private readonly NpgsqlConnection _connection;
        public AuthorRepository()
        {
            _connection = new NpgsqlConnection("Host=localhost;Port=5432; User Id=postgres; Password=root;Database=Kuharica;");
        }
        public async Task<bool> Delete(Guid uuid)
        {
            try
            {
                const string commandText = "DELETE FROM \"Author\" WHERE \"Uuid\" = @uuid";
                using var cmd = _connection.CreateCommand();
                cmd.CommandText = commandText;
                cmd.Parameters.AddWithValue("@Uuid", uuid);
                await _connection.OpenAsync();
                var rowAffected = await cmd.ExecuteNonQueryAsync();
                await _connection.CloseAsync();
                return rowAffected > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task <ServiceResponse<Author>> Get(Guid uuid)
        {
            var response = new ServiceResponse<Author>();   

            string commandText = "SELECT \"FirstName\", \"LastName\"  FROM \"Author\" WHERE \"Uuid\" = @uuid;";

            var command = new NpgsqlCommand(commandText, _connection);

            command.Parameters.AddWithValue("@Uuid", NpgsqlTypes.NpgsqlDbType.Uuid, uuid);

            await _connection.OpenAsync();

            using (var reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    response.Data = new Author
                    {

                        FirstName = reader.GetString(0),
                        LastName = reader.GetString(1),

                    };

                    await _connection.CloseAsync();
                    await _connection.DisposeAsync();
                    
                    response.Success = true;

                    return response;

                }
                else
                {
                    response.Success = false;
                    response.Message = "No author found in DB";
                    return response;
                }
            }
        }

        public async Task <ServiceResponse<List<Author>>> GetAll()
        {
            var response = new ServiceResponse<List<Author>>();
            try
            {
                string commandText = "SELECT * FROM \"Author\" where \"IsActive\" = true;";
                var listFromDB = new List<Author>();
                var command = new NpgsqlCommand(commandText, _connection);

                await _connection.OpenAsync();

                var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
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

                await _connection.CloseAsync();
                await reader.DisposeAsync();

                response.Data = listFromDB;
                response.Success = true;

                return response;
            }
            catch (Exception ex) 
            {
                response.Success = false;
                response.Message = "No data found in database";
                return response;
            }

        }

        public async Task <ServiceResponse<List<Author>>> GetNotActive()
        {
            var response = new ServiceResponse<List<Author>>();

            string commandText = "SELECT * FROM \"Author\" where \"IsActive\" = false;";
            var listFromDB = new List<Author>();
            var command = new NpgsqlCommand(commandText, _connection);

            await _connection.OpenAsync();

            var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
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
            if(listFromDB is null)
            {
                response.Success =false;
                response.Message = "No data in database";

                return response;
            }

            await _connection.CloseAsync();
            await reader.DisposeAsync();

            response.Data = listFromDB;
            response.Success = true;

            return response;
        }

        public async Task<List<AuthorRecipe>> GetRecepiesByAuthorGuid(Guid uuid)
        {
            string commandText = "SELECT \"Author\".\"FirstName\", \"Author\".\"LastName\", \"Recipe\".\"Title\" " +
                "FROM \"Author\"" +
                " INNER JOIN \"AuthorRecipe\" ON \"Author\".\"Id\" = \"AuthorRecipe\".\"AuthorId\"" +
                " INNER JOIN \"Recipe\" ON \"Recipe\".\"Id\" = \"AuthorRecipe\".\"RecipeId\"" +
                " WHERE \"Author\".\"Uuid\" = @uuid";

            var listFromDB = new List<AuthorRecipe>();
            var command = new NpgsqlCommand(commandText, _connection);

            command.Parameters.AddWithValue("@Uuid", NpgsqlTypes.NpgsqlDbType.Uuid, uuid);
            await _connection.OpenAsync();

            var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                listFromDB.Add(new AuthorRecipe
                {
                    FirstName = reader.GetString(0),
                    LastName = reader.GetString(1),
                    Title = reader.GetString(2)

                });
            }
            if (listFromDB is null)
            {
                return null;
            }

            await _connection.CloseAsync();
            await reader.DisposeAsync();

            return listFromDB;
        }

        public async Task <ServiceResponse<Author>> Post(Author item)
        {
            var response = new ServiceResponse<Author>();

            try
            {
                string commandText = "INSERT INTO \"Author\" (\"Uuid\", \"FirstName\", \"LastName\",\"DateOfBirth\", \"Bio\", \"IsActive\", \"DateCreated\", \"DateUpdated\") "
                    + " VALUES (@Uuid, @FirstName, @LastName, @DateOfBirth, @Bio, @IsActive, @DateCreated, @DateUpdated) RETURNING \"Id\"";


                using var cmd = _connection.CreateCommand();
                cmd.CommandText = commandText;
               
                Guid guid = Guid.NewGuid();

                cmd.Parameters.AddWithValue("@Uuid", item.Uuid = guid);

                AddParameters(cmd, item);
                await _connection.OpenAsync();
                var rowAffected = await cmd.ExecuteNonQueryAsync();
                await _connection.CloseAsync();
                await _connection.DisposeAsync();

                response.Data = item;
                response.Success = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Unexpected error at author repository post method! " + ex.Message; 
                return response;
            }
        }

        public async Task <ServiceResponse<Author>> Put(Author item, Guid uuid)
        {
            var response = new ServiceResponse<Author>();
            try
            {
                const string commandText = "UPDATE \"Author\" SET \"Id\" = @Id, \"Uuid\" =@Uuid, \"FirstName\" = @FirstName, " +
                    "\"LastName\" = @LastName, \"DateOfBirth\" = @DateOfBirth, \"Bio\" = @Bio, \"IsActive\" = @IsActive, " +
           "\"DateUpdated\" = @DateUpdated WHERE \"Uuid\" = @Uuid;";



                using var cmd = _connection.CreateCommand();
                cmd.CommandText = commandText;
                AddParameters(cmd, item);

                cmd.Parameters.AddWithValue("@Uuid", uuid);
                cmd.Parameters.AddWithValue("@Id", item.Id);

                await _connection.OpenAsync();
                var rowAffected = await cmd.ExecuteNonQueryAsync();
                await _connection.CloseAsync();
                await _connection.DisposeAsync();

                response.Success = true;
                response.Data = item;

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Unexpected error at author repository put method! " + ex.Message;
                return response;
            }
        }

        private void AddParameters(NpgsqlCommand command, Author author)
        {
           
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
