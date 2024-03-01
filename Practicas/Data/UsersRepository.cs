using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using Practicas.Entities;
using Practicas.Interfaces;

namespace Practicas.Data
{
    public class UsersRepository(IConfiguration configuration) : IUsersRepository
    {
        private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection");

        public async Task<List<Usuario>> GetUsers()
        {
            List<Usuario> usuarios = [];
            using (SqlConnection connection = new(_connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new("SELECT id,name,email,password FROM Users", connection);
                using DbDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    Usuario usuario = new()
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("id")),
                        Name = reader.GetString(reader.GetOrdinal("name")),
                        Email = reader.GetString(reader.GetOrdinal("email")),
                        Password = reader.GetString(reader.GetOrdinal("password"))
                    };
                    usuarios.Add(usuario);
                }
            }

            return usuarios;
        }
        public async Task<Usuario> GetUserByEmail(string email)
        {
            Usuario usuario;
            using (SqlConnection connection = new(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT id,name,email,password FROM Users WHERE email = @Email";
                SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@Email",email);
                using DbDataReader reader = await command.ExecuteReaderAsync();
                if (!reader.HasRows) return new Usuario{Id = -1,Name = "",Email="",Password=""};
                await reader.ReadAsync();
                usuario = new()
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    Email = reader.GetString(reader.GetOrdinal("email")),
                    Password = reader.GetString(reader.GetOrdinal("password"))
                };
            }

            return usuario;
        }
    }
}
