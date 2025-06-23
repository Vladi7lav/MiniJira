using MiniJira.Domain.Entities;

namespace MiniJira.Repository;

public interface ILogRepository
{
    Task CreateLog(long taskId, int userId, string description, CancellationToken cancellationToken);
    Task<LogEntity[]> GetLogsByTaskId(int taskId, CancellationToken cancellationToken);
}
