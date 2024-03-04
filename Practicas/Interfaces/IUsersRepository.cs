using System.Data;
using Microsoft.AspNetCore.Mvc;
using Practicas.DTOs;

namespace Practicas.Interfaces
{
    public interface IUsersRepository
    {
        Task<List<User>> GetUsers();
        Task<User> PutUserByEmail(User user);
        Task<ActionResult> DeleteUserByEmail(string email);
        Task<User> PostUser(User user);
        Task<User> GetUserByEmail(string email);
    }
}