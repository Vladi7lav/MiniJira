using MiniJira.Domain.Entities;

namespace MiniJira.Repository;

public interface IUserRepository
{
    Task CreateUser(UserEntity user, CancellationToken cancellationToken);
    Task ChangeUserPassword(int userId, string newPassword, CancellationToken cancellationToken);
    Task<int?> GetUserIdByLogin(string login, CancellationToken cancellationToken);
    Task<UserEntity?> GetUserByLogin(string login, CancellationToken cancellationToken);
    Task<UserBaseInfoEntity[]> GetUsersWithoutActiveTasks(CancellationToken cancellationToken);
    Task UpdateUserName(string login, string? newFirstName, string? newMidName, string? newLastName, CancellationToken cancellationToken);
    Task DeleteUser(string login, CancellationToken cancellationToken);
}
