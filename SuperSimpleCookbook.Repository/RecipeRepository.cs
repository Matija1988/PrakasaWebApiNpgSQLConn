using Microsoft.VisualBasic.FileIO;
using Npgsql;
using SuperSimpleCookbook.Common;
using SuperSimpleCookbook.Model;
using SuperSimpleCookbook.Model.Model;
using SuperSimpleCookbook.Repository.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSimpleCookbook.Repository
{
    public class RecipeRepository : IRepositoryRecipe<Recipe>
    {

        private readonly NpgsqlConnection _connection;
        public RecipeRepository()
        {
            _connection = new NpgsqlConnection("Host=localhost;Port=5432; User Id=postgres; Password=root;Database=Kuharica;");
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                const string commandText = "DELETE FROM \"Recipe\" WHERE \"Id\" = @id";
                using var cmd = _connection.CreateCommand();
                cmd.CommandText = commandText;
                cmd.Parameters.AddWithValue("@Id", id);
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

        public async Task<ServiceResponse<Recipe>> GetAsync(int id)
        {
            var response = new ServiceResponse<Recipe>();

            string commandText = "SELECT \"Title\", \"Subtitle\"  FROM \"Recipe\" WHERE \"Id\" = @id;";

            var command = new NpgsqlCommand(commandText, _connection);

            command.Parameters.AddWithValue("@Id", id);

            _connection.Open();

            using (var reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    response.Data = new Recipe
                    {

                        Title = reader.GetString(0),
                        Subtitle = reader.GetString(1),

                    };

                    _connection.Close();
                    await _connection.DisposeAsync();

                    response.Success = true;

                    return response;

                }
                else
                {
                    response.Success = false;
                    response.Message = "No recipe with id: " + id + " in database!";
                    return response;
                }
            }
        }

        public async Task<ServiceResponse<List<Recipe>>> GetAllAsync()
        {
            var response = new ServiceResponse<List<Recipe>>();

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
                response.Success = false;
                response.Message = "No data in database";
                return response;
            }

            _connection.Close();
            await reader.DisposeAsync();

            response.Success = true;
            response.Data = listFromDB;

            return response;
        }

        public async Task<ServiceResponse<List<Recipe>>> GetNotActiveAsync()
        {
            var response = new ServiceResponse<List<Recipe>>();

            string commandText = "SELECT * FROM \"Recipe\" where \"IsActive\" = false;";

            var listFromDB = new List<Recipe>();
            var command = new NpgsqlCommand(commandText, _connection);

            _connection.Open();

            var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                listFromDB.Add(new Recipe
                {

                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Subtitle = reader.GetString(2),
                    Text = reader.GetString(3),
                    IsActive = reader.GetBoolean(4),
                    DateCreated = reader.GetDateTime(5),
                    DateUpdated = reader.GetDateTime(6)

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

        public async Task<ServiceResponse<Recipe>> PostAsync(Recipe item)
        {
            var response = new ServiceResponse<Recipe>();
            try
            {
                string commandText = "INSERT INTO \"Recipe\" (\"Title\", \"Subtitle\", \"Text\",\"IsActive\",\"DateCreated\", \"DateUpdated\") "
                    + " VALUES (@Title, @Subtitle, @Text, @IsActive, @DateCreated, @DateUpdated) RETURNING \"Id\"";


                using var cmd = _connection.CreateCommand();
                cmd.CommandText = commandText;

                cmd.Parameters.AddWithValue("@DateCreated", item.DateCreated);

                AddParameters(cmd, item);
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
                response.Message = "Unexpected error in recipe repository post method " + ex.Message;
                return response;
            }
        }

        public async Task<ServiceResponse<Recipe>> PutAsync(Recipe item, int id)
        {
            var response = new ServiceResponse<Recipe>();

            try
            {
                const string commandText = "UPDATE \"Recipe\" SET \"Id\" = @Id, \"Title\" =@Title, \"Subtitle\" = @Subtitle, " +
                    "\"IsActive\" = @IsActive, \"DateUpdated\" = @DateUpdated WHERE \"Id\" = @id;";

                using var cmd = _connection.CreateCommand();
                cmd.CommandText = commandText;
                AddParameters(cmd, item);

                int tempId = id;
                item.Id = tempId;

                cmd.Parameters.AddWithValue("@Id", item.Id);

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
                response.Message = "Unexpected error at recipe repository put method " + ex.Message;
                return response;
            }
        }

        public async Task<ServiceResponse<List<Recipe>>>
            GetRecipeWithFilterPagingAndSortAsync(FilterForRecipe filter, Paging paging, SortOrder sort)
        {
            var response = new ServiceResponse<List<Recipe>>();

            StringBuilder query = ReturnConditionString(filter, paging, sort);

            var listFromDB = new List<Recipe>();

            var command = new NpgsqlCommand(query.ToString(), _connection);

            SetFilterParams(command, filter, paging, sort);

            _connection.Open();

            var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
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

        private void SetFilterParams
            (NpgsqlCommand command, FilterForRecipe filter, Paging paging, SortOrder sort)
        {
            if (!string.IsNullOrWhiteSpace(filter.Title))
            {
                command.Parameters.AddWithValue("@Title", "%" + filter.Title + "%");
            }
            if (!string.IsNullOrWhiteSpace(filter.Subtitle))
            {
                command.Parameters.AddWithValue("@Subtitle", "%" + filter.Subtitle + "%");
            }
            if (filter.DateCreated is not null)
            {
                command.Parameters.AddWithValue("@DateCreated", filter.DateCreated.Value.Date);
            }

            //if(!string.IsNullOrWhiteSpace(filter.AuthorName))
            //{
            //    command.Parameters.AddWithValue("@Author", filter.AuthorName);
            //}


            if (!string.IsNullOrWhiteSpace(sort.OrderBy))
            {
                command.Parameters.AddWithValue("@OrderBy", sort.OrderBy);
            }
            if (!string.IsNullOrWhiteSpace(sort.OrderDirection))
            {
                command.Parameters.AddWithValue("@OrderDirection", sort.OrderDirection);
            }


            command.Parameters.AddWithValue("@PageSize", paging.PageSize);
            command.Parameters.AddWithValue("@PageNumber", paging.PageNumber);

        }

        private StringBuilder ReturnConditionString(FilterForRecipe filter, Paging paging, SortOrder sort)
        {
            StringBuilder query = new StringBuilder("SELECT * FROM \"Recipe\" WHERE \"IsActive\" = true");

            //if (!string.IsNullOrWhiteSpace(filter.AuthorName)) 
            //{
            //    query.Clear();
            //    query.Append("SELECT \"Author\".\"FirstName\", \"Author\".\"LastName\", \"Recipe\".\"Title\" " +
            //    "FROM \"Author\"" +
            //    " INNER JOIN \"AuthorRecipe\" ON \"Author\".\"Id\" = \"AuthorRecipe\".\"AuthorId\"" +
            //    " INNER JOIN \"Recipe\" ON \"Recipe\".\"Id\" = \"AuthorRecipe\".\"RecipeId\"" +
            //    " WHERE \"Author\".\"FirstName\" Like @FirstName");
            //}

            if (!string.IsNullOrWhiteSpace(filter.Title))
            {
                query.Append(" AND \"Title\" LIKE @Title");
            }

            if (!string.IsNullOrWhiteSpace(filter.Subtitle))
            {
                query.Append(" AND \"Subtitle\" LIKE @Subtitle");
            }

            if (filter.DateCreated is not null)
            {
                query.Append(" AND DATE(\"DateCreated\") = @DateCreated");
            }

            if (!string.IsNullOrEmpty(sort.OrderDirection) && !string.IsNullOrEmpty(sort.OrderBy))
            {
                query.Append($" ORDER BY \"{sort.OrderBy}\"  {sort.OrderDirection} ");

            }

            if (int.IsPositive(paging.PageSize) && paging.PageNumber > 0)
            {
                int page = (paging.PageNumber - 1) * paging.PageSize;

                query.Append(" LIMIT @PageSize OFFSET " + page);

            }

            return query;
        }

        private void AddParameters(NpgsqlCommand cmd, Recipe item)
        {
            cmd.Parameters.AddWithValue("@Title", item.Title);
            cmd.Parameters.AddWithValue("@Subtitle", item.Subtitle);
            cmd.Parameters.AddWithValue("@Text", item.Text);
            cmd.Parameters.AddWithValue("@IsActive", item.IsActive);
            cmd.Parameters.AddWithValue("@DateUpdated", item.DateUpdated);
        }


    }
}


