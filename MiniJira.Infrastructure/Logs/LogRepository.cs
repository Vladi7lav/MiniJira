using Dapper;
using MiniJira.Domain.Entities;
using MiniJira.Repository;

namespace MiniJira.Infrastructure.Logs;

public class LogRepository : ILogRepository
{
    public async Task CreateLog(long taskId, int userId, string description, CancellationToken cancellationToken)
    {
        var command = new CommandDefinition(
            """
            INSERT INTO tasks_logs (
                             task_id,
                             changed_task_user_id,
                             description
                             ) values
            (@taskId, @changedTaskUserId, @description)
            """,
            new
            {
                taskId = taskId,
                changedTaskUserId = userId,
                description = description
            },
            cancellationToken: cancellationToken);
        using var connection = await DatabaseConnectionHelper.GetOpenConnection(cancellationToken);
        await connection.ExecuteAsync(command);
    }

    public async Task<LogEntity[]> GetLogsByTaskId(long taskId, CancellationToken cancellationToken)
    {
        var command = new CommandDefinition(
            """
            select
                task_id as TaskId,
                description as Description,
                u.login as UserNameWhoChanged,
                created_at as ActionDate
            from tasks_logs tl
            left join users u on tl.changed_task_user_id = u.user_id
            where task_id = @taskId;
            """,
            new
            {
                taskId = taskId,
            },
            cancellationToken: cancellationToken);
        using var connection = await DatabaseConnectionHelper.GetOpenConnection(cancellationToken);
        return (await connection.QueryAsync<LogEntity>(command)).ToArray();
    }
}
