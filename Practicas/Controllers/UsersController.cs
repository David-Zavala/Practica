using System.Data;
using Microsoft.AspNetCore.Mvc;
using Practicas.Data;
using Practicas.Entities;
using Practicas.Interfaces;

namespace Practicas.Controllers
{
    public class UsersController(IUsersRepository usersRepository) : BaseApiController
    {
        private readonly IUsersRepository _usersRepository = usersRepository;

        [HttpGet]
        public async Task<ActionResult<List<Usuario>>> GetUsers()
        {
            return await _usersRepository.GetUsers();
        }

        [HttpPost("login")]
        public async Task<ActionResult<Usuario>> Login(string email, string password)
        {
            Usuario usuario = await _usersRepository.GetUserByEmail(email);
            if (usuario.Id == -1) return Unauthorized("That email is not registered, try using another or register first");
            if (usuario.Password != password) return Unauthorized("Invalid Password");
            return usuario;
        }
    }
}