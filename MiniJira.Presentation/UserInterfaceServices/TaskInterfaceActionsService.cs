using MiniJira.Domain.Entities;
using MiniJira.DomainServices.Converters;
using MiniJira.DomainServices.Services.Tasks;
using MiniJira.Presentation.Helpers;
using MiniJira.Presentation.Interfaces;

namespace MiniJira.Presentation.UserInterfaceServices;

public class TaskInterfaceActionsService : ITaskInterfaceActionsService
{
    private readonly ITaskService _taskService;

    public TaskInterfaceActionsService(ITaskService taskService)
    {
        _taskService = taskService;
    }
    
    public async Task CreateTask(UserEntity currentUser, CancellationToken cancellationToken)
    {
        var taskName = TaskInterfaceHelper.GetTaskName();
        var description = TaskInterfaceHelper.GetDescription();
        var projectNumber = TaskInterfaceHelper.GetProjectNumber();
        var manager = TaskInterfaceHelper.GetManager();
        var customer = TaskInterfaceHelper.GetCustomer();
        var task = new TaskEntity
        {
            ProjectNumber = projectNumber,
            Name = taskName,
            Description = description,
            Manager = manager,
            Customer = customer
        };
        await _taskService.CreateTask(task, currentUser.Id, cancellationToken);

        Console.WriteLine("Задача успешно создана.");
    }
    
    public async Task UpdateTask(UserEntity currentUser, CancellationToken cancellationToken)
    {
        var id = TaskInterfaceHelper.GetTaskId();
        Console.WriteLine("Далее, если не хотите изменять указанный параметр нажимайте \"Enter\"");
        Console.WriteLine("Введите новое название задачи:");
        var taskName = Console.ReadLine();
        Console.WriteLine("Введите новое описание:");
        var description = Console.ReadLine();
        var projectNumber = InputHelper.ReadInputNullableInt("Введите новый номер проекта:");
        Console.WriteLine("Введите логин нового менеджера:");
        var managerLogin = Console.ReadLine();
        Console.WriteLine("Введите логин нового исполнителя:");
        var customerLogin = Console.ReadLine();
        var result = await _taskService.UpdateTask(
            id,
            currentUser.Id,
            projectNumber,
            taskName,
            description,
            managerLogin,
            customerLogin,
            cancellationToken);

        if (!result.Success)
        {
            Console.WriteLine(result.ErrorMessage);
            return;
        }
        
        Console.WriteLine("Задача успешно обновлена");
    }
    
    public async Task UpdateTaskStatusById(UserEntity currentUser, CancellationToken cancellationToken)
    {
        var id = TaskInterfaceHelper.GetTaskId();
        var newStatus = InputHelper.ReadTaskStatus()!.Value;
        var result = await _taskService.UpdateTaskStatus(id, newStatus, currentUser.Id, cancellationToken);
        if (!result.Success)
        {
            Console.WriteLine(result.ErrorMessage);
            return;
        }
        
        Console.WriteLine("Задача успешно обновлена");
    }
    
    public async Task<TaskEntity?> GetTaskByTaskId(CancellationToken cancellationToken)
    {
        var taskId = TaskInterfaceHelper.GetTaskId();
        var task = await _taskService.GetTaskById(taskId, cancellationToken);
        if (task == null)
        {
            Console.WriteLine($"Задача с Id = {taskId} не найдена");
            return null;
        }
        ShowTaskInfo(task);
        return task;
    }
    
    public async Task GetTasksByCustomerLogin(CancellationToken cancellationToken)
    {
        var login = InputHelper.ReadInputString("Введите логин исполнителя: ");
        var status = InputHelper.ReadTaskStatus(
            "Введите номер статуса задачи или \"Enter\" для того что бы не использовать ограничение по статусу при поиске: ",
            true);
        var tasks = await _taskService.GetTasksByCustomerLogin(login, status, cancellationToken);

        if (tasks.Length == 0)
        {
            Console.WriteLine($"Задачи для пользователя с логином: {login} {(status == null ? string.Empty : $"и статусом: {status.Value.ToRuString()}")} не найдены.");
            return;
        }

        foreach (var task in tasks)
        {
            ShowTaskShortInfo(task);
        }
    }

    public async Task GetTasksByCustomerId(UserEntity currentUser, CancellationToken cancellationToken)
    {
        var status = InputHelper.ReadTaskStatus(
            "Введите номер статуса задачи или \"Enter\" для того что бы не использовать ограничение по статусу при поиске: ",
            true);
        var tasks = await _taskService.GetTasksByCustomerId(currentUser.Id, status, cancellationToken);

        if (tasks.Length == 0)
        {
            Console.WriteLine($"Задачи для исполнителя с Id: {currentUser.Id} {(status == null ? string.Empty : $"и статусом: {status.Value.ToRuString()}")} не найдены.");
            return;
        }

        foreach (var task in tasks)
        {
            ShowTaskShortInfo(task);
        }
    }
    
    public async Task GetTasksByManagerId(UserEntity currentUser, CancellationToken cancellationToken)
    {
        var status = InputHelper.ReadTaskStatus(
            "Введите номер статуса задачи или \"Enter\" для того что бы не использовать ограничение по статусу при поиске: \n",
            true);
        var tasks = await _taskService.GetTasksByManagerId(currentUser.Id, status, cancellationToken);

        if (tasks.Length == 0)
        {
            Console.WriteLine($"Задачи для менеджера с Id: {currentUser.Id} {(status == null ? string.Empty : $"и статусом: {status.Value.ToRuString()}")} не найдены.");
            Environment.Exit(0);
        }

        foreach (var task in tasks)
        {
            ShowTaskShortInfo(task);
        }
    }

    public async Task GetTasksByStatus(CancellationToken cancellationToken)
    {
        var status = InputHelper.ReadTaskStatus()!.Value;
        var tasks = await _taskService.GetTasksByStatus(status, cancellationToken);

        if (tasks.Length == 0)
        {
            Console.WriteLine($"Задачи со статусом {status.ToRuString()} не найдены.");
            return;
        }

        foreach (var task in tasks)
        {
            ShowTaskShortInfo(task);
        }
    }
    
    public async Task GetTasksByPartNameOrName(CancellationToken cancellationToken)
    {
        var partName = InputHelper.ReadInputString("Введит название или часть названия задачи: ");
        var tasks = await _taskService.GetTasksByPartNameOrName(partName, cancellationToken);

        if (tasks.Length == 0)
        {
            Console.WriteLine($"Задачи с названием содержащим текст \"{partName}\" не найдены.");
            return;
        }

        foreach (var task in tasks)
        {
            ShowTaskShortInfo(task);
        }
    }

    public async Task GetTaskByIdWithLogs(CancellationToken cancellationToken)
    {
        var taskId = TaskInterfaceHelper.GetTaskId();
        var taskWithLogs = await _taskService.GetTaskByIdWithLogs(taskId, cancellationToken);

        if (taskWithLogs.Task == null)
        {
            Console.WriteLine($"Задача с Id = {taskId} не найдена.");
        }
        else
        {
            ShowTaskInfo(taskWithLogs.Task);
        }
        
        ShowLogs(taskWithLogs.Logs);
    }

    public async Task GetLogsByTaskId(CancellationToken cancellationToken)
    {
        var taskId = TaskInterfaceHelper.GetTaskId();
        var logs = await _taskService.GetLogsByTaskId(taskId, cancellationToken);
        ShowLogs(logs);
    }

    public async Task DeleteTaskById(UserEntity currentUser, CancellationToken cancellationToken)
    {
        var id = TaskInterfaceHelper.GetTaskId();
        await _taskService.DeleteTask(id, currentUser.Id, cancellationToken);
        Console.WriteLine("Удаление прошло успешно.");
    }

    private static void ShowTaskInfo(TaskEntity task)
    {
        Console.WriteLine($"\nID: {task.Id}, Название: {task.Name}, Номер проекта: {task.ProjectNumber}, Статус: {task.Status.ToRuString()}");
        Console.WriteLine($"Менеджер: {task.Manager}, Исполнитель: {task.Customer}");
        Console.WriteLine($"Дата создания: {task.CreatedAt}, Дата последнего обновления: {(task.LastUpdatedAt?.ToString() ?? "Отсутствует")}");
        Console.WriteLine($"Описание: {task.Description}");
    }
    
    private static void ShowTaskShortInfo(TaskEntity task)
    {
        Console.WriteLine($"\nID: {task.Id}, Название: {task.Name}, Номер проекта: {task.ProjectNumber}, Статус: {task.Status.ToRuString()}, Исполнитель: {task.Customer}");
    }
    
    private static void ShowLogs(LogEntity[] logs)
    {
        if (logs.Length == 0)
        {
            Console.WriteLine("Логи не найдены");
        }
        
        Console.WriteLine("\n");
        foreach (var log in logs.OrderBy(l => l.ActionDate))
        {
            Console.WriteLine($"Время: {log.ActionDate} Пользователь: {log.UserNameWhoChanged}, Описание: {log.Description}");
        }
    }
}