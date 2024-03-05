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
        public async Task<ActionResult<User>> Login(string email, string password)
        {
            User usuario = await _usersRepository.GetUserByEmail(email);
            if (usuario.Email == "") return Unauthorized("That email is not registered, try using another or register first");
            // if (usuario.Password != password) return Unauthorized("Invalid Password");
            return usuario;
        }
    }
}