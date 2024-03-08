using Practicas.DTOs;

namespace Practicas.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(UserLogin user);
    }
}