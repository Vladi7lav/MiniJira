using MiniJira.Domain.Entities;
using MiniJira.Domain.Enums;

namespace MiniJira.Repository;

public interface ITaskRepository
{
    Task CreateTask(TaskEntity task, int managerId, CancellationToken cancellationToken);

    Task UpdateTaskStatus(long taskId, TaskStatuses status, CancellationToken cancellationToken);

    Task UpdateTask(
        long taskId,
        int? projectNumber,
        string? name,
        string? description,
        int? managerId,
        int? customerId,
        CancellationToken cancellationToken);

    Task<TaskEntity[]> GetTasksByCustomerId(int customerId, TaskStatuses? status, CancellationToken cancellationToken);
    Task<TaskEntity[]> GetTasksByCustomerLogin(string customerLogin, TaskStatuses? status, CancellationToken cancellationToken);
    Task<TaskEntity[]> GetTasksByManagerId(int managerId, TaskStatuses? status, CancellationToken cancellationToken);
    Task<TaskEntity[]> GetTasksByStatus(TaskStatuses status, CancellationToken cancellationToken);
    Task<TaskEntity[]> GetTasksByPartNameOrName(string taskPartName, CancellationToken cancellationToken);
    Task<TaskEntity?> GetTaskById(long taskId, CancellationToken cancellationToken);

    Task DeleteTask(long taskId, CancellationToken cancellationToken);
}
