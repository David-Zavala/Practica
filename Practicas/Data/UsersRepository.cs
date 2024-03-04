using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
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

                SqlCommand command = new("SELECT name,email,password FROM Users", connection);
                using DbDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    User usuario = new()
                    {
                        Email = reader.GetString(reader.GetOrdinal("email")),
                        Name = reader.GetString(reader.GetOrdinal("name")),
                        Password = reader.GetString(reader.GetOrdinal("password"))
                    };
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
                if (!reader.HasRows) return new User{Name = "",Email="",Password=""};
                await reader.ReadAsync();
                usuario = new()
                {
                    Email = reader.GetString(reader.GetOrdinal("email")),
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    Password = reader.GetString(reader.GetOrdinal("password")),
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
                
                string query;
                if (user.BirthDate == null) query = "UPDATE Users SET name = @Name, password = @Password WHERE Id = 1";
                else query = "UPDATE Users SET name = @Name, password = @Password, birthdate = @Birthdate WHERE Id = 1";
                
                SqlCommand command = new(query, connection, transaction);
                
                command.Parameters.AddWithValue("@Name",user.Name); 
                command.Parameters.AddWithValue("@Password",user.Password);
                if (user.BirthDate != null) command.Parameters.AddWithValue("@Birthdate",user.BirthDate);
                
                try {await command.ExecuteNonQueryAsync();}
                catch (SqlException){await transaction.RollbackAsync(); return new User{Name = "",Email="",Password=""};}

                transaction.Commit();
                transaction.Dispose();
                connection.Close();
            }
            return new User{
                Email = user.Email,
                Name = user.Name,
                Password = user.Password,
            };
        }

        public Task<ActionResult> DeleteUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task<User> PostUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
