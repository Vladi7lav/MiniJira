using System.Threading;
using System.Threading.Tasks;
using MiniJira.Domain.Entities;
using MiniJira.Domain.Enums;

namespace MiniJira.Repository
{
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
        Task<TaskEntity[]> GetTasksByCustomerLogin(int customerLogin, TaskStatuses? status, CancellationToken cancellationToken);
        Task<TaskEntity[]> GetTasksByManagerId(int managerId, TaskStatuses? status, CancellationToken cancellationToken);
        Task<TaskEntity?> GetTaskById(int taskId, CancellationToken cancellationToken);
        
        Task DeleteTask(long taskId, CancellationToken cancellationToken);
    }
}