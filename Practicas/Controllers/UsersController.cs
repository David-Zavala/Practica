using Microsoft.AspNetCore.Mvc;
using Practicas.DTOs;
using Practicas.Interfaces;

namespace Practicas.Controllers
{
    public class UsersController(IUsersRepository usersRepository) : BaseApiController
    {
        private readonly IUsersRepository _usersRepository = usersRepository;

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            return await _usersRepository.GetUsers();
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(UserLogin user)
        {
            UserLogin usuario = await _usersRepository.Login(user);
            if (usuario.Email == "NoEmail") return Unauthorized("That email is not registered, try using another or register first");
            if (usuario.Email == "NoPassword") return Unauthorized("Invalid Password");
            return new User{
                Name = usuario.Name,
                Email = usuario.Email,
                BirthDate = usuario.BirthDate,
            };
        }
    }
}