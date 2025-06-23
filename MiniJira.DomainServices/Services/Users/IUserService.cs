using MiniJira.Domain.Entities;

namespace MiniJira.DomainServices.Services.Users;

public interface IUserService
{
    Task<bool> LoginAsync(string username, string password, CancellationToken cancellationToken);
    Task CreateUser(UserEntity user, CancellationToken cancellationToken);
    Task ChangeUserPassword(int userId, string newPassword, CancellationToken cancellationToken);
    Task<int?> TryGetUserIdByLogin(string login, CancellationToken cancellationToken);
    Task<UserEntity?> TryGetUserByLogin(string login, CancellationToken cancellationToken);
    Task<UserShortInfoEntity[]> GetUsersWithoutActiveTasks(CancellationToken cancellationToken);
    Task UpdateUserName(int userId, string newFirstName, string newMidName, string newLastName, CancellationToken cancellationToken);
    Task DeleteUser(string login, CancellationToken cancellationToken);
}
