using MiniJira.Domain.Entities;
using MiniJira.DomainServices.SharedContracts;

namespace MiniJira.DomainServices.Services.Users;

public interface IUserService
{
    bool CheckPassword(UserEntity user, string password);
    Task CreateUser(UserEntity user, CancellationToken cancellationToken);
    Task<SimpleOperationResult> ChangeUserPassword(UserEntity user, string newPassword, CancellationToken cancellationToken);
    Task<bool> CheckUserExistsByLogin(string login, CancellationToken cancellationToken);
    Task<int?> GetUserIdByLogin(string login, CancellationToken cancellationToken);
    Task<UserEntity?> GetUserByLogin(string login, CancellationToken cancellationToken);
    Task<UserBaseInfoEntity[]> GetUsersWithoutActiveTasks(CancellationToken cancellationToken);
    Task<SimpleOperationResult> UpdateUserName(string login, string? newFirstName, string? newMidName, string? newLastName, CancellationToken cancellationToken);
    Task<SimpleOperationResult> UpdateUserName(UserEntity user, string? newFirstName, string? newMidName, string? newLastName, CancellationToken cancellationToken);
    Task DeleteUser(string login, CancellationToken cancellationToken);
}
