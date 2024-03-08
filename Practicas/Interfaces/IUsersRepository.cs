using Practicas.DTOs;

namespace Practicas.Interfaces
{
    public interface IUsersRepository
    {
        Task<List<User>> GetUsers();
        Task<UserLogin> Login(UserLogin user);
        Task<User> GetUserByEmail(string email);
        Task<User> PutUserByEmail(User user);
        Task<int> DeleteUserByEmail(string email);
        Task<User> PostUser(UserLogin user);
    }
}