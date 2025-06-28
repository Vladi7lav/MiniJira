using Dapper;
using MiniJira.Domain.Entities;
using MiniJira.Repository;

namespace MiniJira.Infrastructure.Users;

public class UserRepository : IUserRepository
{
    public async Task CreateUser(UserEntity user, CancellationToken cancellationToken)
    {
        var command = new CommandDefinition(
            """
            INSERT INTO users (
                login,
                password,
                "role",
                firstname,
                midname,
                lastname
            ) values
            (@login, @password, @role::user_roles, @firstname, @midname, @lastname)
            returning id;
            """,
            new
            {
                login = user.Login,
                password = user.Password,
                role = user.Role.ToDbRole(),
                firstname = user.Firstname,
                midname = user.Midname,
                lastname = user.Lastname,
            },
            cancellationToken: cancellationToken);
        using var connection = await DatabaseConnectionHelper.GetOpenConnection(cancellationToken);
        var newId = connection.QueryFirstOrDefaultAsync<int>(command).Result;
        user.Id = newId;
    }

    public async Task ChangeUserPassword(int userId, string newPassword, CancellationToken cancellationToken)
    {
        var command = new CommandDefinition(
            """
            UPDATE users
            SET password = @password, last_change_password_date = now()
            WHERE userId = @userId;
            """,
            new
            {
                userId = userId,
                password = newPassword
            },
            cancellationToken: cancellationToken);
        using var connection = await DatabaseConnectionHelper.GetOpenConnection(cancellationToken);
        await connection.ExecuteAsync(command);
    }

    public async Task<int?> GetUserIdByLogin(string login, CancellationToken cancellationToken)
    {
        var command = new CommandDefinition(
            """
            SELECT id
            FROM users
            WHERE login = @login;
            """,
            new
            {
                login = login,
            },
            cancellationToken: cancellationToken);
        using var connection = await DatabaseConnectionHelper.GetOpenConnection(cancellationToken);
        return await connection.QueryFirstOrDefaultAsync<int?>(command);
    }

    public async Task<UserEntity?> GetUserByLogin(string login, CancellationToken cancellationToken)
    {
        var command = new CommandDefinition(
            """
            SELECT
                id,
                login,
                password,
                "role" as Role,
                firstname,
                midname,
                lastname,
                created_at as CreatedAt,
                last_change_password_date as LastChangePasswordDate
            FROM users
            WHERE login = @login;
            """,
            new
            {
                login = login,
            },
            cancellationToken: cancellationToken);
        using var connection = await DatabaseConnectionHelper.GetOpenConnection(cancellationToken);
        return (await connection.QueryFirstOrDefaultAsync<UserDb?>(command))?.ToUserEntity();
    }

    public async Task<UserBaseInfoEntity[]> GetUsersWithoutActiveTasks(CancellationToken cancellationToken)
    {
        var command = new CommandDefinition(
            """
            WITH customersWithActiveTasks as
            (
                SELECT DISTINCT customer_id
                FROM tasks
                WHERE
                    customer_id is not null
                    and status in ('to_do', 'in_progress')
            )
            SELECT
                id,
                login,
                firstname,
                midname,
                lastname
            FROM users as u
            WHERE u.id not in (select customer_id from customersWithActiveTasks);
            """,
            cancellationToken: cancellationToken);
        using var connection = await DatabaseConnectionHelper.GetOpenConnection(cancellationToken);
        return (await connection.QueryAsync<UserBaseInfoEntity>(command)).ToArray();
    }

    public async Task UpdateUserName(
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
            throw new Exception("For update user name, must be provided FirstName, MidName or LastName");
        }

        var queryParameters = new DynamicParameters();
        var setters = new List<string>();
        if (!string.IsNullOrWhiteSpace(newFirstName))
        {
            setters.Add("firstname = @newFirstname");
            queryParameters.Add("newFirstname", newFirstName);
        }
        if (!string.IsNullOrWhiteSpace(newMidName))
        {
            setters.Add("midname = @newMidname");
            queryParameters.Add("newMidname", newMidName);
        }
        if (!string.IsNullOrWhiteSpace(newLastName))
        {
            setters.Add("lastname = @newLastname");
            queryParameters.Add("newLastname", newLastName);
        }

        var queryText = "UPDATE users SET " + string.Join(", ", setters) + " WHERE login = @login;";
        queryParameters.Add("login", login);

        var command = new CommandDefinition(
            queryText,
            queryParameters,
            cancellationToken: cancellationToken);
        using var connection = await DatabaseConnectionHelper.GetOpenConnection(cancellationToken);
        await connection.ExecuteAsync(command);
    }

    public async Task DeleteUser(string login, CancellationToken cancellationToken)
    {
        var command = new CommandDefinition(
            """
            DELETE users
            WHERE login = @login;
            """,
            new
            {
                login = login,
            },
            cancellationToken: cancellationToken);
        using var connection = await DatabaseConnectionHelper.GetOpenConnection(cancellationToken);
        await connection.ExecuteAsync(command);
    }
}
