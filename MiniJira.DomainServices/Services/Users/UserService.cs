using MiniJira.Domain.Entities;
using MiniJira.DomainServices.Helpers;
using MiniJira.DomainServices.SharedContracts;
using MiniJira.Repository;

namespace MiniJira.DomainServices.Services.Users;

internal sealed class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public bool CheckPassword(UserEntity user, string password)
    {
        return PasswordHelper.VerifyHashedPassword(user.Password, password);
    }

    public async Task CreateUser(UserEntity user, CancellationToken cancellationToken)
    {
        await _userRepository.CreateUser(user, cancellationToken);
    }

    public async Task<SimpleOperationResult> ChangeUserPassword(UserEntity user, string newPassword, CancellationToken cancellationToken)
    {
        if (PasswordHelper.VerifyHashedPassword(user.Password, newPassword))
        {
            return new SimpleOperationResult(false, "Пароль должен отличаться от предыдущего");
        }
        
        var hashedPassword = PasswordHelper.HashPassword(newPassword);
        await _userRepository.ChangeUserPassword(user.Id, hashedPassword, cancellationToken);
        user.Password = hashedPassword;
        return new SimpleOperationResult(true);
    }

    public async Task<bool> CheckUserExistsByLogin(string login, CancellationToken cancellationToken)
    {
        return (await _userRepository.GetUserIdByLogin(login, cancellationToken)) != null;
    }

    public async Task<int?> GetUserIdByLogin(string login, CancellationToken cancellationToken)
    {
        return await _userRepository.GetUserIdByLogin(login, cancellationToken);
    }

    public async Task<UserEntity?> GetUserByLogin(string login, CancellationToken cancellationToken)
    {
        return await _userRepository.GetUserByLogin(login, cancellationToken);
    }

    public async Task<UserBaseInfoEntity[]> GetUsersWithoutActiveTasks(CancellationToken cancellationToken)
    {
        return await _userRepository.GetUsersWithoutActiveTasks(cancellationToken);
    }

    public async Task<SimpleOperationResult> UpdateUserName(
        string login, 
        string? newFirstName, 
        string? newMidName, 
        string? newLastName,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(newFirstName)
            && string.IsNullOrWhiteSpace(newMidName)
            && string.IsNullOrWhiteSpace(newLastName))
        {
            return new SimpleOperationResult(false, 
                "Для обновления пользователя должен быть задан хотя бы один из параметров: Имя, Фамилия, Отчество");
        }
        
        await _userRepository.UpdateUserName(login, newFirstName, newMidName, newLastName, cancellationToken);
        return new SimpleOperationResult(true);
    }

    public async Task<SimpleOperationResult> UpdateUserName(
        UserEntity user, 
        string? newFirstName, 
        string? newMidName, 
        string? newLastName,
        CancellationToken cancellationToken)
    {
        var result = await UpdateUserName(user.Login, newFirstName, newMidName, newLastName, cancellationToken);
        if (!result.Success)
        {
            return result;
        }

        if (!string.IsNullOrWhiteSpace(newFirstName))
        {
            user.Firstname = newFirstName;
        }
        if (!string.IsNullOrWhiteSpace(newMidName))
        {
            user.Midname = newMidName;
        }
        if (!string.IsNullOrWhiteSpace(newLastName))
        {
            user.Lastname = newLastName;
        }     
        return result;
    }
    
    public async Task DeleteUser(string login, CancellationToken cancellationToken)
    {
        await _userRepository.DeleteUser(login, cancellationToken);
    }
}
