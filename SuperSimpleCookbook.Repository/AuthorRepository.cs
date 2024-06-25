using Npgsql;
using SuperSimpleCookbook.Common;
using SuperSimpleCookbook.Model;
using SuperSimpleCookbook.Model.Model;
using SuperSimpleCookbook.Repository.Common.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace SuperSimpleCookbook.Repository
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

            string commandText = "SELECT * FROM \"Author\" WHERE \"Uuid\" = @uuid;";

            var command = new NpgsqlCommand(commandText, _connection);

            command.Parameters.AddWithValue("@Uuid", NpgsqlTypes.NpgsqlDbType.Uuid, uuid);

            _connection.Open();

            using (var reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    response.Data = new Author
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

                    _connection.Close();
                    await _connection.DisposeAsync();

                    response.Success = true;
                    
                    return response;

                }
                else
                {
                        
                }
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


                cmd.Parameters.AddWithValue("@Uuid", item.Uuid);
                cmd.Parameters.AddWithValue("@DateCreated", item.DateCreated);

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

        public async Task<ServiceResponse<List<Author>>>
            GetAuthorWithFilterPagingAndSortAsync(FilterForAuthor filter, Paging paging, SortOrder sort)
        {
            var response = new ServiceResponse<List<Author>>();

            var batch = new NpgsqlBatch(_connection);

            var selectCommand = new NpgsqlBatchCommand();

            var countCommand = new NpgsqlBatchCommand();

            StringBuilder query = ReturnConditionString(selectCommand, filter, paging, sort);

            StringBuilder countQuery = ReturnCountQueryBuilder(countCommand, filter);

            //StringBuilder totalCountQuery = ReturnCountString(); 

            var listFromDB = new List<Author>();

            selectCommand.CommandText = query.ToString();

            countCommand.CommandText = countQuery.ToString();

            batch.BatchCommands.Add(selectCommand);
            batch.BatchCommands.Add(countCommand);

            
            _connection.Open();

            var reader = await batch.ExecuteReaderAsync();

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
                    Role = new Role
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Name = reader.GetString(reader.GetOrdinal("Name"))
                    },
                    Username = reader.GetString(reader.GetOrdinal("Username")),
                    Password = reader.GetString(reader.GetOrdinal("Password"))

                });
                
            }
            await reader.NextResultAsync();

            if (await reader.ReadAsync())
            {
                response.TotalCount = reader.GetInt32(0);
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



        #region Extensions

        private StringBuilder ReturnCountQueryBuilder
            (NpgsqlBatchCommand countCommand, FilterForAuthor filter)
        {
            StringBuilder query = new StringBuilder
                ("SELECT COUNT(*) FROM \"Author\" WHERE \"IsActive\" = true ");

            if(!string.IsNullOrEmpty(filter.FirstName))
            {
                countCommand.Parameters.AddWithValue("@FirstName", "%" + filter.FirstName + "%");
                query.Append(" AND \"FirstName\" LIKE @FirstName ");
            }
            if (!string.IsNullOrEmpty(filter.FirstName))
            {
                countCommand.Parameters.AddWithValue("@LastName", "%" + filter.LastName + "%");
                query.Append(" AND \"LastName\" LIKE @LastName ");
            }
            if (filter.DateOfBirth != null) 
            { 
                countCommand.Parameters.AddWithValue("@DateOfBirth", 
                    NpgsqlTypes.NpgsqlDbType.Date, filter.DateOfBirth);
                query.Append(" AND \"DateOfBirth\" = @DateOfBirth ");
            }
            if (filter.DateCreated != null) 
            {
                countCommand.Parameters.AddWithValue("@DateCreated",
                    NpgsqlTypes.NpgsqlDbType.Date, filter.DateCreated);
                query.Append(" AND \"DateCreated\" = @DateCreated ");
            }
           
            return query;
        }

        private StringBuilder ReturnConditionString(NpgsqlBatchCommand command, 
            FilterForAuthor filter, 
            Paging paging, 
            SortOrder sort)
        {
            StringBuilder query = new StringBuilder(
                "SELECT a.*, b.* " +
                "FROM " +
                "\"Author\" a " +
                "INNER JOIN \"Role\" b ON a.\"RoleId\" = b.\"Id\" " +
                "WHERE \"IsActive\" = true");



            if (!string.IsNullOrWhiteSpace(filter.FirstName))
            {
                query.Append(" AND \"FirstName\" LIKE @FirstName");
                command.Parameters.AddWithValue("@FirstName", "%" + filter.FirstName + "%");
            }

            if (!string.IsNullOrWhiteSpace(filter.LastName))
            {
                query.Append(" AND \"LastName\" LIKE @LastName");
                command.Parameters.AddWithValue("@LastName", "%" + filter.LastName + "%");
            }

            if (filter.DateOfBirth is not null)
            {
                query.Append(" AND DATE(\"DateOfBirth\") = @DateOfBirth");
                command.Parameters.AddWithValue("@DateOfBirth", filter.DateOfBirth.Value.Date);
            }

            if (filter.DateCreated is not null)
            {
                query.Append(" AND DATE(\"DateCreated\") = @DateCreated");
                command.Parameters.AddWithValue("@DateCreated", filter.DateCreated.Value.Date);
            }


            if (!string.IsNullOrEmpty(sort.OrderDirection) && !string.IsNullOrEmpty(sort.OrderBy))
            {
                query.Append($" ORDER BY \"{sort.OrderBy}\"  {sort.OrderDirection} ");
                command.Parameters.AddWithValue("@OrderBy", sort.OrderBy);
            }

            if (!string.IsNullOrWhiteSpace(sort.OrderDirection))
            {
                command.Parameters.AddWithValue("@OrderDirection", sort.OrderDirection);
            }

            if (int.IsPositive(paging.PageSize) && paging.PageNumber > 0)
            {
                int page = (paging.PageNumber - 1) * paging.PageSize;

                query.Append(" LIMIT @PageSize OFFSET " + page);

            }

            command.Parameters.AddWithValue("@PageSize", paging.PageSize);
            command.Parameters.AddWithValue("@PageNumber", paging.PageNumber);


            return query;
        }

        //private void SetFilterParams
        //    (NpgsqlCommand command, FilterForAuthor filter, Paging paging, SortOrder sort)
        //{
        //    if (!string.IsNullOrWhiteSpace(filter.FirstName))
        //    {
        //        command.Parameters.AddWithValue("@FirstName", "%" + filter.FirstName + "%");
        //    }
        //    if (!string.IsNullOrWhiteSpace(filter.LastName))
        //  s  {
        //        command.Parameters.AddWithValue("@LastName", "%" + filter.LastName + "%");
        //    }
        //    if (filter.DateOfBirth is not null)
        //    {
        //        command.Parameters.AddWithValue("@DateOfBirth", filter.DateOfBirth.Value.Date);
        //    }
        //    if (filter.DateCreated is not null)
        //    {
        //        command.Parameters.AddWithValue("@DateCreated", filter.DateCreated.Value.Date);
        //    }
        //    if (!string.IsNullOrWhiteSpace(sort.OrderBy))
        //    {
        //        command.Parameters.AddWithValue("@OrderBy", sort.OrderBy);
        //    }
        //    if (!string.IsNullOrWhiteSpace(sort.OrderDirection))
        //    {
        //        command.Parameters.AddWithValue("@OrderDirection", sort.OrderDirection);
        //    }

        //    command.Parameters.AddWithValue("@PageSize", paging.PageSize);
        //    command.Parameters.AddWithValue("@PageNumber", paging.PageNumber);

        //}
        private void AddParameters(NpgsqlCommand command, Author author)
        {
            command.Parameters.AddWithValue("@FirstName", author.FirstName);
            command.Parameters.AddWithValue("@LastName", author.LastName);
            command.Parameters.AddWithValue("@DateOfBirth", author.DateOfBirth);
            command.Parameters.AddWithValue("@Bio", author.Bio);
            command.Parameters.AddWithValue("@IsActive", author.IsActive);
            command.Parameters.AddWithValue("@DateUpdated", author.DateUpdated);

        }

        #endregion
    }
}
