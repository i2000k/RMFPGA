using Domain.Entities;

namespace Application.Interfaces;

public interface IAuthService
{
    string CreateToken(User user);
}