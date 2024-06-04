using Microsoft.VisualBasic.FileIO;
using Npgsql;
using SuperSimpleCookbook.Model;
using SuperSimpleCookbook.Model.Model;
using SuperSimpleCookbook.Repository.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSimpleCookbook.Repository.RecipeRepository
{
    public class RecipeRepository : IRepositoryRecipe<Recipe>
    {

        private readonly NpgsqlConnection _connection;
        public RecipeRepository()
        {
            _connection = new NpgsqlConnection("Host=localhost;Port=5432; User Id=postgres; Password=root;Database=Kuharica;");
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                const string commandText = "DELETE FROM \"Recipe\" WHERE \"Id\" = @id";
                using var cmd = _connection.CreateCommand();
                cmd.CommandText = commandText;
                cmd.Parameters.AddWithValue("@Id", id);
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

        public async Task <ServiceResponse<Recipe>> Get(int id)
        {
            var response = new ServiceResponse<Recipe>();

            string commandText = "SELECT \"Title\", \"Subtitle\"  FROM \"Recipe\" WHERE \"Id\" = @id;";

            var command = new NpgsqlCommand(commandText, _connection);

            command.Parameters.AddWithValue("@Id", id);

            await _connection.OpenAsync();

            using (var reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    response.Data = new Recipe
                    {

                        Title = reader.GetString(0),
                        Subtitle = reader.GetString(1),

                    };

                    await _connection.CloseAsync();
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

        public async Task <ServiceResponse<List<Recipe>>> GetAll()
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
                return null;
            }

            _connection.Close();
            await reader.DisposeAsync();

            response.Success = true;
            response.Data = listFromDB;

            return response;
        }





        public async Task <ServiceResponse<List<Recipe>>> GetNotActive()
        {
            var response = new ServiceResponse <List<Recipe>>();

            string commandText = "SELECT * FROM \"Recipe\" where \"IsActive\" = false;";
            
            var listFromDB = new List<Recipe>();
            var command = new NpgsqlCommand(commandText, _connection);

            await _connection.OpenAsync();

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

            await _connection.CloseAsync();
            await reader.DisposeAsync();

            response.Data = listFromDB;
            response.Success = true;

            return response;
        }

        public async Task <ServiceResponse<Recipe>> Post(Recipe item)
        {
            var response = new ServiceResponse<Recipe>();
            try
            {
                string commandText = "INSERT INTO \"Recipe\" (\"Title\", \"Subtitle\", \"Text\",\"IsActive\",\"DateCreated\", \"DateUpdated\") "
                    + " VALUES (@Title, @Subtitle, @Text, @IsActive, @DateCreated, @DateUpdated) RETURNING \"Id\"";


                using var cmd = _connection.CreateCommand();
                cmd.CommandText = commandText;

                AddParameters(cmd, item);
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
                response.Message = "Error in recipe repository post method " + ex.Message;
                return response;
            }
        }

        public async Task<ServiceResponse<Recipe>> Put(Recipe item, int id)
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
                response.Message = "Update failed " + ex.Message;
                return response;
            }
        }
        private void AddParameters(NpgsqlCommand cmd, Recipe item)
        {
            cmd.Parameters.AddWithValue("@Title", item.Title);
            cmd.Parameters.AddWithValue("@Subtitle", item.Subtitle);
            cmd.Parameters.AddWithValue("@Text", item.Text);
            cmd.Parameters.AddWithValue("@IsActive", item.IsActive);
            cmd.Parameters.AddWithValue("@DateCreated", item.DateCreated);
            cmd.Parameters.AddWithValue("@DateUpdated", item.DateUpdated);
        }

    }
}


