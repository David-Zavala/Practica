using System.Data.Common;
using System.Data.SqlClient;
using Practicas.DTOs;
using Practicas.Interfaces;

namespace Practicas.Data
{
    public class DocsRepository(IConfiguration configuration) : IDocsRepository
    {
        private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection");

        public Task<int> DeleteDoc(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Doc> GetDoc(int id)
        {
            Doc doc;
            using (SqlConnection connection = new(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT id,name,email,birthdate,education,docpath FROM Docs WHERE id = @Id";
                SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@EId",id);
                using DbDataReader reader = await command.ExecuteReaderAsync();
                if (!reader.HasRows) return new Doc{Id = -1,Name = "",Email = "",BirthDate = "",Education = "",DocPath = ""};
                await reader.ReadAsync();
                doc = MapDoc(reader);
            }
            return doc;
        }

        public async Task<List<Doc>> GetDocs()
        {
            List<Doc> docs = [];
            using (SqlConnection connection = new(_connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new("SELECT id,name,email,birthdate,education,docpath FROM Docs", connection);
                using DbDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    Doc doc = MapDoc(reader);
                    docs.Add(doc);
                }
                return docs;
            }
        }

        public async Task<Doc> PostDoc(Doc doc)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                await connection.OpenAsync();
                SqlTransaction transaction = connection.BeginTransaction();
                
                string query = "INSERT INTO Docs (name, email, birthdate, education, DocPath) VALUES (@Name,@Email,@Birthdate,@Education,@DocPath);";
                
                SqlCommand command = new(query, connection, transaction);
                
                command.Parameters.AddWithValue("@Name",doc.Name); 
                command.Parameters.AddWithValue("@Email",doc.Email); 
                command.Parameters.AddWithValue("@Birthdate",doc.BirthDate);
                command.Parameters.AddWithValue("@Education",doc.Education);
                command.Parameters.AddWithValue("@DocPath",doc.DocPath);
                
                try 
                {
                    await command.ExecuteNonQueryAsync();
                }
                catch (SqlException)
                {
                    await transaction.RollbackAsync();
                    transaction.Dispose();
                    connection.Close();
                    return new Doc{};
                }

                transaction.Commit();
                transaction.Dispose();
                connection.Close();
            }
            return doc;
        }

        public Task<Doc> PutDoc(Doc doc)
        {
            throw new NotImplementedException();
        }

        private static Doc MapDoc(DbDataReader reader) => new()
        {
            Id = reader.GetInt32(reader.GetOrdinal("id")),
            Name = reader.GetString(reader.GetOrdinal("name")),
            Email = reader.GetString(reader.GetOrdinal("email")),
            BirthDate = reader.GetDateTime(reader.GetOrdinal("birthdate")).Date.ToString("yyyy-MM-dd"),
            Education = reader.GetString(reader.GetOrdinal("education")),
            DocPath = reader.GetString(reader.GetOrdinal("docpath"))
        };
    }
}