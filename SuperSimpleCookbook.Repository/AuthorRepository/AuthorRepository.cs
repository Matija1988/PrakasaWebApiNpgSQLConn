using Npgsql;
using SuperSimpleCookbook.Model;
using SuperSimpleCookbook.Repository.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSimpleCookbook.Repository.AuthorRepository
{
    public class AuthorRepository : IRepositoryAuthor<Author>
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

        public async Task<Author> Get(Guid uuid)
        {
            string commandText = "SELECT \"FirstName\", \"LastName\"  FROM \"Author\" WHERE \"Uuid\" = @uuid;";

            var command = new NpgsqlCommand(commandText, _connection);

            command.Parameters.AddWithValue("@Uuid", NpgsqlTypes.NpgsqlDbType.Uuid, uuid);

            await _connection.OpenAsync();

            using (var reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    var author = new Author
                    {

                        FirstName = reader.GetString(0),
                        LastName = reader.GetString(1),

                    };

                    await _connection.CloseAsync();
                    await _connection.DisposeAsync();
                    return author;

                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<List<Author>> GetAll()
        {
            try
            {
                string commandText = "SELECT * FROM \"Author\" where \"IsActive\" = true;";
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
                        DateUpdated = reader.GetDateTime(8),

                    });
                }

                await _connection.CloseAsync();
                await reader.DisposeAsync();

                return listFromDB;
            }
            catch (Exception ex) 
            { 
                throw new Exception(ex.Message);
            }

        }

        public async Task<List<Author>> GetNotActive()
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
            if(listFromDB is null)
            {
                return null;
            }

            await _connection.CloseAsync();
            await reader.DisposeAsync();

            return listFromDB;
        }

        public async Task<Author> Post(Author item)
        {
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
                return item;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Author> Put(Author item, Guid uuid)
        {
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
                return item;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);  
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
