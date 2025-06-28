using MiniJira.Domain.Entities;
using MiniJira.Domain.Enums;
using MiniJira.Presentation.CustomAttributes;

namespace MiniJira.Presentation.Interfaces;

public interface IUserInterfaceActionsService
{
    [AccessByRole(UserRoles.Manager, "Создать нового пользователя", 1)]
    Task AddUser(CancellationToken cancellationToken);
    
    [AccessByRole([UserRoles.Customer, UserRoles.Manager], "Получить информацию о пользователе по логину", 2)]
    Task GetUserByLogin(CancellationToken cancellationToken);
    
    [AccessByRole(UserRoles.Manager, "Получить всех сотрудников без активных задач", 3)]
    Task GetUsersWithoutActiveTasks(CancellationToken cancellationToken);
    
    [AccessByRole(UserRoles.Manager, "Обноваить ФИО пользователя по логину", 4)]
    Task UpdateUser(CancellationToken cancellationToken);
    
    [AccessByRole([UserRoles.Customer, UserRoles.Manager], "Изменить пароль", 5)]
    Task ChangeUserPassword(UserEntity currentUser, CancellationToken cancellationToken);
    
    [AccessByRole(UserRoles.Manager, "Удалить пользователя по логину", 6)]
    Task DeleteUser(CancellationToken cancellationToken);
}