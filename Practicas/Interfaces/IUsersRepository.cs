using System.Data;
using Practicas.Entities;

namespace Practicas.Interfaces
{
    public interface IUsersRepository
    {
        Task<List<Usuario>> GetUsers();
        Task<Usuario> GetUserByEmail(string email);
    }
}