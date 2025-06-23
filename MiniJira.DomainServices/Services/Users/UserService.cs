using MiniJira.Repository;

namespace MiniJira.DomainServices.Services.Users;

internal sealed class UserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }


}
