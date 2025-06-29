using MiniJira.Domain.Entities;
using MiniJira.Domain.Enums;
using MiniJira.Presentation.CustomAttributes;

namespace MiniJira.Presentation.Interfaces;

public interface ITaskInterfaceActionsService
{
    [AccessByRole(UserRoles.Manager, "Создать задачу", 1)]
    Task CreateTask(UserEntity currentUser, CancellationToken cancellationToken);

    [AccessByRole(UserRoles.Manager, "Обновить задачу", 2)]
    Task UpdateTask(UserEntity currentUser, CancellationToken cancellationToken);

    [AccessByRole([UserRoles.Customer, UserRoles.Manager], "Обновить статус задачи по id задачи", 3)]
    Task UpdateTaskStatusById(UserEntity currentUser, CancellationToken cancellationToken);

    [AccessByRole([UserRoles.Customer, UserRoles.Manager], "Получить ифнормацию о задаче по id", 4)]
    Task<TaskEntity?> GetTaskByTaskId(CancellationToken cancellationToken);

    [AccessByRole(UserRoles.Manager, "Получить ифнормацию о задаче по логину исполнителя", 5)]
    Task GetTasksByCustomerLogin(CancellationToken cancellationToken);

    [AccessByRole(UserRoles.Customer, "Получить информацию о назначенных задачах", 6)]
    Task GetTasksByCustomerId(UserEntity currentUser, CancellationToken cancellationToken);

    [AccessByRole(UserRoles.Manager, "Получить информацию о назначенных задачах", 6)]
    Task GetTasksByManagerId(UserEntity currentUser, CancellationToken cancellationToken);

    [AccessByRole([UserRoles.Customer, UserRoles.Manager], "Получить информацию о задачах по статусу", 7)]
    Task GetTasksByStatus(CancellationToken cancellationToken);

    [AccessByRole([UserRoles.Customer, UserRoles.Manager],
        "Получить информацию о задачах по названию или части названия", 8)]
    Task GetTasksByPartNameOrName(CancellationToken cancellationToken);

    [AccessByRole([UserRoles.Customer, UserRoles.Manager],
        "Получить информацию о задаче с историей изменений по Id задачи", 9)]
    Task GetTaskByIdWithLogs(CancellationToken cancellationToken);

    [AccessByRole([UserRoles.Customer, UserRoles.Manager], "Получить историю изменений задачи по её Id", 10)]
    Task GetLogsByTaskId(CancellationToken cancellationToken);

    [AccessByRole(UserRoles.Manager, "Удалить задачу по Id", 11)]
    Task DeleteTaskById(UserEntity currentUser, CancellationToken cancellationToken);
}