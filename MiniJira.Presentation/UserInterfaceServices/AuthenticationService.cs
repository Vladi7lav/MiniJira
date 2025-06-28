using MiniJira.Domain.Entities;
using MiniJira.DomainServices.Services.Users;
using MiniJira.Presentation.Helpers;
using MiniJira.Presentation.Interfaces;

namespace MiniJira.Presentation.UserInterfaceServices;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserService _userService;

    public AuthenticationService(IUserService userService)
    {
        _userService = userService;
    }
    
    public async Task<UserEntity> AuthenticateUser(CancellationToken cancellationToken)
    {
        UserEntity? user;

        do
        {
            var login = UserInterfaceHelper.GetLogin();
            user = await _userService.GetUserByLogin(login, cancellationToken);
            if (user == null)
            {
                Console.WriteLine("Пользователь с данным логином не найден, повторите ввод: ");
            }
        }while(user == null);

        while (true)
        {
            var password = UserInterfaceHelper.GetPassword();
            if (_userService.CheckPassword(user, password))
            {
                return user;
            }

            Console.WriteLine("Неверный пароль, повторите ввод: ");
        }
    }
}