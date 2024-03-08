using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Practicas.DTOs;
using Practicas.Interfaces;

namespace Practicas.Data
{
    public class UsersRepository(IConfiguration configuration) : IUsersRepository
    {
        private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection");

        public async Task<List<User>> GetUsers()
        {
            List<User> usuarios = [];
            using (SqlConnection connection = new(_connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new("SELECT name,email,birthdate FROM Users", connection);
                using DbDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    User usuario = MapUser(reader);
                    usuarios.Add(usuario);
                }
            }

            return usuarios;
        }
        public async Task<User> GetUserByEmail(string email)
        {
            User usuario;
            using (SqlConnection connection = new(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT email,name,password,birthdate FROM Users WHERE email = @Email";
                SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@Email",email);
                using DbDataReader reader = await command.ExecuteReaderAsync();
                if (!reader.HasRows) return new User{};
                await reader.ReadAsync();
                usuario = new()
                {
                    Email = reader.GetString(reader.GetOrdinal("email")),
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    BirthDate = reader.GetDateTime(reader.GetOrdinal("birthdate")).Date.ToString("yyyy-MM-dd")
                };
            }
            return usuario;
        }

        public async Task<User> PutUserByEmail(User user)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                await connection.OpenAsync();
                SqlTransaction transaction = connection.BeginTransaction();
                
                string query = "UPDATE Users SET name = @Name, password = @Password, birthdate = @Birthdate WHERE Id = 1";
                
                SqlCommand command = new(query, connection, transaction);
                
                command.Parameters.AddWithValue("@Name",user.Name);
                command.Parameters.AddWithValue("@Birthdate",user.BirthDate);
                
                try 
                {
                    await command.ExecuteNonQueryAsync();
                }
                catch (SqlException)
                {
                    await transaction.RollbackAsync();
                    transaction.Dispose();
                    connection.Close();
                    return new User{};
                }

                transaction.Commit();
                transaction.Dispose();
                connection.Close();
            }
            return new User{
                Email = user.Email,
                Name = user.Name,
                BirthDate = user.BirthDate
            };
        }

        public async Task<int> DeleteUserByEmail(string email)
        {
            using (SqlConnection connection = new(_connectionString)){
                await connection.OpenAsync();
                SqlTransaction transaction = connection.BeginTransaction();

                string query = "DELETE FROM Users WHERE email = @Email";

                SqlCommand command = new(query, connection, transaction);

                command.Parameters.AddWithValue("@Email",email);

                try 
                {
                    await command.ExecuteNonQueryAsync();
                }
                catch (SqlException)
                {   
                    await transaction.RollbackAsync();
                    transaction.Dispose();
                    connection.Close();
                    return 0;
                }

                transaction.Commit();
                transaction.Dispose();
                connection.Close();
            }
            return 1;
        }

        public async Task<User> PostUser(UserLogin user)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                await connection.OpenAsync();
                SqlTransaction transaction = connection.BeginTransaction();
                
                string query = "INSERT INTO Users (name, email, password, birthdate) VALUES (@Name,@Email,@Password,@Birthdate);";
                
                SqlCommand command = new(query, connection, transaction);
                
                command.Parameters.AddWithValue("@Name",user.Name); 
                command.Parameters.AddWithValue("@Email",user.Email); 
                command.Parameters.AddWithValue("@Birthdate",user.BirthDate);
                
                try 
                {
                    await command.ExecuteNonQueryAsync();
                }
                catch (SqlException)
                {
                    await transaction.RollbackAsync();
                    transaction.Dispose();
                    connection.Close();
                    return new User{};
                }

                transaction.Commit();
                transaction.Dispose();
                connection.Close();
            }
            return new User{
                Email = user.Email,
                Name = user.Name,
                BirthDate = user.BirthDate,
            };
        }

        private static User MapUser(DbDataReader reader) => new()
        {
            Email = reader.GetString(reader.GetOrdinal("email")),
            Name = reader.GetString(reader.GetOrdinal("name")),
            BirthDate = reader.GetDateTime(reader.GetOrdinal("birthdate")).Date.ToString("yyyy-MM-dd")
        };

        public async Task<UserLogin> Login(UserLogin user)
        {
            UserLogin retrievedUser;
            using (SqlConnection connection = new(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT email,name,password,birthdate FROM Users WHERE email = @Email";
                SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@Email",user.Email);
                using DbDataReader reader = await command.ExecuteReaderAsync();
                if (!reader.HasRows) return new UserLogin{Email = "NoEmail",Password=""};
                await reader.ReadAsync();
                retrievedUser = new()
                {
                    Email = reader.GetString(reader.GetOrdinal("email")),
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    BirthDate = reader.GetDateTime(reader.GetOrdinal("birthdate")).Date.ToString("yyyy-MM-dd"),
                    Password = reader.GetString(reader.GetOrdinal("password"))
                };
                if (user.Password != retrievedUser.Password) return new UserLogin{Email = "NoPassword", Password=""};
            }
            return retrievedUser;
        }
    }
}
