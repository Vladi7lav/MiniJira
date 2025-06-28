using MiniJira.Domain.Entities;
using MiniJira.Domain.Enums;
using MiniJira.DomainServices.SharedContracts;

namespace MiniJira.DomainServices.Services.Tasks;

public interface ITaskService
{
    Task CreateTask(TaskEntity task, int managerId, CancellationToken cancellationToken);

    Task<SimpleOperationResult> UpdateTaskStatus(long taskId, TaskStatuses status, int userId, CancellationToken cancellationToken);

    Task<SimpleOperationResult> UpdateTask(
        long taskId,
        int userId,
        int? projectNumber,
        string? name,
        string? description,
        string? managerLogin,
        string? customerLogin,
        CancellationToken cancellationToken);

    Task<TaskEntity[]> GetTasksByCustomerId(int customerId, TaskStatuses? status, CancellationToken cancellationToken);
    Task<TaskEntity[]> GetTasksByCustomerLogin(string customerLogin, TaskStatuses? status, CancellationToken cancellationToken);
    Task<TaskEntity[]> GetTasksByManagerId(int managerId, TaskStatuses? status, CancellationToken cancellationToken);
    Task<TaskEntity[]> GetTasksByStatus(TaskStatuses status, CancellationToken cancellationToken);
    Task<TaskEntity[]> GetTasksByPartNameOrName(string taskPartName, CancellationToken cancellationToken);
    Task<TaskEntity?> GetTaskById(long taskId, CancellationToken cancellationToken);
    Task<TaskWithLogsEntity> GetTaskByIdWithLogs(long taskId, CancellationToken cancellationToken);
    Task<LogEntity[]> GetLogsByTaskId(long taskId, CancellationToken cancellationToken);

    Task DeleteTask(long taskId, int userId, CancellationToken cancellationToken);
}