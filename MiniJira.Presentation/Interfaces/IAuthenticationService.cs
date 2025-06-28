using MiniJira.Domain.Entities;

namespace MiniJira.Presentation.Interfaces;

public interface IAuthenticationService
{
    Task<UserEntity> AuthenticateUser(CancellationToken cancellationToken);
}