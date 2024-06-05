using Npgsql;
using SuperSimpleCookbook.Common;
using SuperSimpleCookbook.Model;
using SuperSimpleCookbook.Model.Model;
using SuperSimpleCookbook.Repository.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
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
        public async Task<bool> DeleteAsync(Guid uuid)
        {
            try
            {

                const string commandText = "DELETE FROM \"Author\" WHERE \"Uuid\" = @uuid";
                using var cmd = _connection.CreateCommand();
                cmd.CommandText = commandText;
                cmd.Parameters.AddWithValue("@Uuid", uuid);
                _connection.Open();
                var rowAffected = await cmd.ExecuteNonQueryAsync();
                _connection.Close();
                return rowAffected > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<ServiceResponse<Author>> GetAsync(Guid uuid)
        {
            var response = new ServiceResponse<Author>();

            string commandText = "SELECT \"FirstName\", \"LastName\"  FROM \"Author\" WHERE \"Uuid\" = @uuid;";

            var command = new NpgsqlCommand(commandText, _connection);

            command.Parameters.AddWithValue("@Uuid", NpgsqlTypes.NpgsqlDbType.Uuid, uuid);

            _connection.Open();

            using (var reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    response.Data = new Author
                    {

                        FirstName = reader.GetString(0),
                        LastName = reader.GetString(1),

                    };

                    _connection.Close();
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

        public async Task<ServiceResponse<List<Author>>> GetAllAsync()
        {
            var response = new ServiceResponse<List<Author>>();
            try
            {
                string commandText = "SELECT * FROM \"Author\" where \"IsActive\" = true;";
                var listFromDB = new List<Author>();
                var command = new NpgsqlCommand(commandText, _connection);

                _connection.Open();

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

                _connection.Close();
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

        public async Task<ServiceResponse<List<Author>>> GetNotActiveAsync()
        {
            var response = new ServiceResponse<List<Author>>();

            string commandText = "SELECT * FROM \"Author\" where \"IsActive\" = false;";
            var listFromDB = new List<Author>();
            var command = new NpgsqlCommand(commandText, _connection);

            _connection.Open();

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
            if (listFromDB is null)
            {
                response.Success = false;
                response.Message = "No data in database";

                return response;
            }

            _connection.Close();
            await reader.DisposeAsync();

            response.Data = listFromDB;
            response.Success = true;

            return response;
        }

        public async Task<ServiceResponse<List<AuthorRecipe>>> GetRecepiesByAuthorGuidAsync(Guid uuid)
        {
            var response = new ServiceResponse<List<AuthorRecipe>>();

            string commandText = "SELECT \"Author\".\"FirstName\", \"Author\".\"LastName\", \"Recipe\".\"Title\" " +
                "FROM \"Author\"" +
                " INNER JOIN \"AuthorRecipe\" ON \"Author\".\"Id\" = \"AuthorRecipe\".\"AuthorId\"" +
                " INNER JOIN \"Recipe\" ON \"Recipe\".\"Id\" = \"AuthorRecipe\".\"RecipeId\"" +
                " WHERE \"Author\".\"Uuid\" = @uuid";

            var listFromDB = new List<AuthorRecipe>();
            var command = new NpgsqlCommand(commandText, _connection);

            command.Parameters.AddWithValue("@Uuid", NpgsqlTypes.NpgsqlDbType.Uuid, uuid);
            _connection.Open();

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
                response.Success = false;
                response.Message = "No data in database!";
                return response;
            }

            _connection.Close();
            await reader.DisposeAsync();

            response.Data = listFromDB;
            response.Success = true;

            return response;
        }

        public async Task<ServiceResponse<Author>> PostAsync(Author item)
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
                _connection.Open();
                var rowAffected = await cmd.ExecuteNonQueryAsync();
                _connection.Close();
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

        public async Task<ServiceResponse<Author>> PutAsync(Author item, Guid uuid)
        {
            var response = new ServiceResponse<Author>();
            try
            {
                const string commandText = "UPDATE \"Author\" SET  \"Uuid\" =@Uuid, \"FirstName\" = @FirstName, " +
                    "\"LastName\" = @LastName, \"DateOfBirth\" = @DateOfBirth, \"Bio\" = @Bio, \"IsActive\" = @IsActive, " +
           "\"DateUpdated\" = @DateUpdated WHERE \"Uuid\" = @Uuid;";

                using var cmd = _connection.CreateCommand();
                cmd.CommandText = commandText;
                AddParameters(cmd, item);

                cmd.Parameters.AddWithValue("@Uuid", uuid);


                _connection.Open();
                var rowAffected = await cmd.ExecuteNonQueryAsync();
                _connection.Close();
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

        public async Task<ServiceResponse<List<Author>>> GetAuthorWithFilterPageingAndSort(FilterForAuthor filter, Paging paging, SortOrder sort)
        {
            var response = new ServiceResponse<List<Author>>();

            StringBuilder query = new StringBuilder("SELECT * FROM \"Author\" WHERE \"IsActive\" = true");

            var listFromDB = new List<Author>();

            if (!string.IsNullOrWhiteSpace(filter.FirstName))
            {
                query.Append(" AND \"FirstName\" LIKE @FirstName");
            }

            if (!string.IsNullOrWhiteSpace(filter.LastName))
            {
                query.Append(" AND \"LastName\" LIKE @LastName");
            }

            if (filter.DateOfBirth is not null)
            {
                query.Append(" AND DATE(\"DateOfBirth\") = @DateOfBirth");
            }

            var command = new NpgsqlCommand(query.ToString(), _connection);

            if (!string.IsNullOrWhiteSpace(filter.FirstName))
            {
                command.Parameters.AddWithValue("@FirstName", "%" + filter.FirstName + "%");
            }
            if (!string.IsNullOrWhiteSpace(filter.LastName))
            {
                command.Parameters.AddWithValue("@LastName", "%" + filter.LastName + "%");
            }
            if (filter.DateOfBirth is not null)
            {
                command.Parameters.AddWithValue("@DateOfBirth", filter.DateOfBirth.Value.Date);
            }


            _connection.Open();

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

                _connection.Close();
                await reader.DisposeAsync();

            if (listFromDB is not null)
            {

                response.Data = listFromDB;
                response.Success = true;

                return response;
            }
            else
            {
                response.Message = "No data in database";
                response.Success = false;
                return response;
            }
        }

        private StringBuilder ReturnConditionString(FilterForAuthor filter)
        {
            StringBuilder condition = new StringBuilder("SELECT * FROM \"Author\"");



            return condition;
        }
      
    }
}
