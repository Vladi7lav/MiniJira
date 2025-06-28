using MiniJira.Domain.Entities;
using MiniJira.DomainServices.Converters;
using MiniJira.DomainServices.Helpers;
using MiniJira.DomainServices.Services.Users;
using MiniJira.Presentation.Helpers;
using MiniJira.Presentation.Interfaces;

namespace MiniJira.Presentation.UserInterfaceServices;

public class UserInterfaceActionsService : IUserInterfaceActionsService
{
    private readonly IUserService  _userService;

    public UserInterfaceActionsService(IUserService userService)
    {
        _userService = userService;
    }

    public async Task AddUser(CancellationToken cancellationToken)
    {
        var login = await GetNewUserLogin(cancellationToken);
        if (login == null)
        {
            return;
        }
        
        var password = UserInterfaceHelper.GetPassword();
        var firstName = UserInterfaceHelper.GetFirstName();
        var lastName = UserInterfaceHelper.GetLastName();
        var midName = UserInterfaceHelper.GetMidName();
        var role = InputHelper.ReadUserRole();

        var user = new UserEntity
        {
            Login = login,
            Password = PasswordHelper.HashPassword(password),
            Firstname = firstName,
            Lastname = lastName,
            Midname = midName,
            Role = role
        };
        await _userService.CreateUser(user, cancellationToken);
        Console.WriteLine($"Пользователь успешно создан. ID: {user.Id}");
    }

    public async Task GetUserByLogin(CancellationToken cancellationToken)
    {
        var login = UserInterfaceHelper.GetLogin();
        var user =  await _userService.GetUserByLogin(login, cancellationToken);
        if (user == null)
        {
            Console.WriteLine($"Пользователь с логином {login} не найден");
            return;
        }
        
        ShowUserInfo(user);
    }

    public async Task UpdateUser(CancellationToken cancellationToken)
    {
        var login = InputHelper.ReadInputString("Введите логин пользователя для которого хотите выполнить обновление: ");
        Console.WriteLine("Далее, если не хотите изменять указанный параметр нажимайте \"Enter\"");
        Console.WriteLine("Введите новое имя:");
        var firstname = Console.ReadLine();
        Console.WriteLine("Введите новую фамилию:");
        var lastname = Console.ReadLine();
        Console.WriteLine("Введите новое отчество:");
        var midname = Console.ReadLine();

        var result = await _userService.UpdateUserName(
            login, 
            firstname, 
            midname, 
            lastname, 
            cancellationToken);

        if (!result.Success)
        {
            Console.WriteLine(result.ErrorMessage);
            return;
        }

        Console.WriteLine("Пользователь успешно обновлён.");
    }

    public async Task ChangeUserPassword(UserEntity currentUser, CancellationToken cancellationToken)
    {
        var password = UserInterfaceHelper.GetPassword(true);
        var result = await _userService.ChangeUserPassword(currentUser, password, cancellationToken);
        if (result.Success)
        {
            Console.WriteLine("Пароль успешно обновлён.");
            return;
        }
        
        Console.WriteLine(result.ErrorMessage);
    }

    public async Task DeleteUser(CancellationToken cancellationToken)
    {
        var login = UserInterfaceHelper.GetLogin();
        await _userService.DeleteUser(login, cancellationToken);
        Console.WriteLine("Пользователь с логином {login} удален.");
    }
    
    public async Task GetUsersWithoutActiveTasks(CancellationToken cancellationToken)
    {
        var users = await _userService.GetUsersWithoutActiveTasks(cancellationToken);
        if (users.Length == 0)
        {
            Console.WriteLine("Пользователи без активных задач не найдены.");
            return;
        }
        
        Console.WriteLine("Найдены следующие пользователи не имеющие активных задач: ");
        foreach (var user in users)
        {
            ShowBaseUserInfo(user);
        }
    }

    private async Task<string?> GetNewUserLogin(CancellationToken cancellationToken)
    {
        while (true)
        {
            var login = UserInterfaceHelper.GetLogin();
            var userExists = await _userService.CheckUserExistsByLogin(login, cancellationToken);
            if (!userExists)
            {
                return login;
            }
            Console.WriteLine("Пользователь с данным логином уже зарегестрирован");
            var yesNo = InputHelper.ReadInputIntFromList(
                "Что бы повторить ввод логина введите 1, для выхода в меню введите 2: ",
                [1, 2]);
            if (yesNo == 2)
            {
                return null;
            }
        }
    }

    private static void ShowUserInfo(UserEntity user)
    {
        ShowBaseUserInfo(user);
        Console.WriteLine($"Роль: {user.Role.ToRuString()}, дата добавления: {user.CreatedAt:yyyy-MM-dd HH:mm:ss}");
    }
    
    private static void ShowBaseUserInfo(UserBaseInfoEntity user)
    {
        Console.WriteLine($"Логин: {user.Login}, ФИО: {user.GetFullname()}");
    }
}