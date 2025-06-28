using System.Text;
using MiniJira.Domain.Entities;
using MiniJira.Domain.Enums;
using MiniJira.DomainServices.Converters;
using MiniJira.DomainServices.SharedContracts;
using MiniJira.Repository;

namespace MiniJira.DomainServices.Services.Tasks;

public class TaskService : ITaskService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogRepository _logRepository;
    private readonly ITaskRepository _taskRepository;

    public TaskService(IUserRepository userRepository, ILogRepository logRepository, ITaskRepository taskRepository)
    {
        _userRepository = userRepository;
        _logRepository = logRepository;
        _taskRepository = taskRepository;
    }

    public async Task CreateTask(TaskEntity task, int userId, CancellationToken cancellationToken)
    {
        await _taskRepository.CreateTask(task, userId, cancellationToken);
        await _logRepository.CreateLog(task.Id, userId, "Задача создана", cancellationToken);
    }

    public async Task<SimpleOperationResult> UpdateTaskStatus(long taskId, TaskStatuses status, int userId, CancellationToken cancellationToken)
    {
        var oldTask = await _taskRepository.GetTaskById(taskId, cancellationToken);
        if (oldTask == null)
        {
            return new SimpleOperationResult(false, "Данная задача была удалена");
        }
        await _taskRepository.UpdateTaskStatus(taskId, status, cancellationToken);
        await _logRepository.CreateLog(taskId, userId, $"Статус переведён в {status.ToRuString()}", cancellationToken);
        return new SimpleOperationResult(true);
    }

    public async Task<SimpleOperationResult> UpdateTask(
        long taskId, 
        int userId,
        int? projectNumber, 
        string? name, 
        string? description, 
        string? managerLogin, 
        string? customerLogin,
        CancellationToken cancellationToken)
    {
        int? newCustomerId = null;
        int? newManagerId = null;
        
        if (!string.IsNullOrWhiteSpace(customerLogin))
        {
            newCustomerId = await _userRepository.GetUserIdByLogin(customerLogin,  cancellationToken);
            if (newCustomerId == null)
            {
                return new SimpleOperationResult(false, $"Пользователь с логином {customerLogin} не найден");
            }
        }
        
        if (!string.IsNullOrWhiteSpace(managerLogin))
        {
            var newManager = await _userRepository.GetUserByLogin(managerLogin,  cancellationToken);
            if (newManager == null)
            {
                return new SimpleOperationResult(false, $"Менеджер с логином {managerLogin} не найден");
            }
            else if (newManager.Role != UserRoles.Manager)
            {
                return new SimpleOperationResult(false, $"Сотрудник с логином {managerLogin} не является менеджером");
            }

            newManagerId = newManager.Id;
        }
        
        var oldTask = await _taskRepository.GetTaskById(taskId, cancellationToken);
        if (oldTask == null)
        {
            return new SimpleOperationResult(false, $"Задача с Id {taskId} не найдена");
        }
        
        var changesDescription = FormTaskChangesDescription(oldTask, projectNumber, name, description, managerLogin, customerLogin);

        if (string.IsNullOrEmpty(changesDescription))
        {
            return new SimpleOperationResult(false, "Для обновления задачи параметры должны отличаться от текущих");
        }
        
        await _taskRepository.UpdateTask(taskId, projectNumber, name, description, newManagerId, newCustomerId, cancellationToken);
        await _logRepository.CreateLog(
            taskId, 
            userId, 
            $"Были внесены следующие изменения: {Environment.NewLine}"
            + changesDescription,
            cancellationToken);

        return new SimpleOperationResult(true);
    }

    public async Task<TaskEntity[]> GetTasksByCustomerId(int customerId, TaskStatuses? status, CancellationToken cancellationToken)
    {
        return await _taskRepository.GetTasksByCustomerId(customerId, status, cancellationToken);
    }

    public async Task<TaskEntity[]> GetTasksByCustomerLogin(string customerLogin, TaskStatuses? status, CancellationToken cancellationToken)
    {
        return await _taskRepository.GetTasksByCustomerLogin(customerLogin, status, cancellationToken);
    }

    public async Task<TaskEntity[]> GetTasksByManagerId(int managerId, TaskStatuses? status, CancellationToken cancellationToken)
    {
        return await _taskRepository.GetTasksByManagerId(managerId, status, cancellationToken);
    }

    public async Task<TaskEntity[]> GetTasksByStatus(TaskStatuses status, CancellationToken cancellationToken)
    {
        return await _taskRepository.GetTasksByStatus(status, cancellationToken);
    }

    public async Task<TaskEntity[]> GetTasksByPartNameOrName(string taskPartName, CancellationToken cancellationToken)
    {
        return await _taskRepository.GetTasksByPartNameOrName(taskPartName, cancellationToken);
    }

    public async Task<TaskEntity?> GetTaskById(long taskId, CancellationToken cancellationToken)
    {
        return await _taskRepository.GetTaskById(taskId, cancellationToken);
    }

    public async Task<TaskWithLogsEntity> GetTaskByIdWithLogs(long taskId, CancellationToken cancellationToken)
    {
        var task =  await _taskRepository.GetTaskById(taskId, cancellationToken);
        var logs = await _logRepository.GetLogsByTaskId(taskId, cancellationToken);
        return new TaskWithLogsEntity
        {
            Task = task,
            Logs = logs
        };
    }
    
    public async Task<LogEntity[]> GetLogsByTaskId(long taskId, CancellationToken cancellationToken)
    {
        return await _logRepository.GetLogsByTaskId(taskId, cancellationToken);
    }

    public async Task DeleteTask(long taskId, int userId, CancellationToken cancellationToken)
    {
        await _taskRepository.DeleteTask(taskId, cancellationToken);
        await _logRepository.CreateLog(taskId, userId, "Задача удалена", cancellationToken);
    }

    private static string? FormTaskChangesDescription(
        TaskEntity oldTask,
        int? projectNumber, 
        string? name, 
        string? description, 
        string? managerLogin, 
        string? customerLogin)
    {
        var changesDescription = new StringBuilder();

        if (projectNumber != null && oldTask.ProjectNumber != projectNumber)
        {
            changesDescription.AppendLine($"Номер проекта до изменений: {oldTask.ProjectNumber}, после: {projectNumber}");
        }
        
        if (!string.IsNullOrWhiteSpace(name) && !oldTask.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
        {
            changesDescription.AppendLine($"Название задачм до изменений: {oldTask.Name}, после: {name}");
        }
        
        if (!string.IsNullOrWhiteSpace(description) && !oldTask.Description.Equals(description, StringComparison.CurrentCultureIgnoreCase))
        {
            changesDescription.AppendLine($"Описание задачм до изменений: {oldTask.Description}, после: {description}");
        }
        
        if (!string.IsNullOrWhiteSpace(managerLogin) && !oldTask.Manager.Equals(managerLogin, StringComparison.CurrentCultureIgnoreCase))
        {
            changesDescription.AppendLine($"Менеджер до изменений: {oldTask.Manager}, после: {managerLogin}");
        }
        
        if (!string.IsNullOrWhiteSpace(customerLogin) && !oldTask.Customer.Equals(customerLogin, StringComparison.CurrentCultureIgnoreCase))
        {
            changesDescription.AppendLine($"Выполняющий сотрудник до изменений: {oldTask.Customer}, после: {customerLogin}");
        }
        
        return changesDescription.Length > 0 ? changesDescription.ToString() : null;
    }
}