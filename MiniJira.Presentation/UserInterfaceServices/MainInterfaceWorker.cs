using MiniJira.Domain.Entities;
using MiniJira.Presentation.Contracts;
using MiniJira.Presentation.Enums;
using MiniJira.Presentation.Helpers;
using MiniJira.Presentation.Interfaces;

namespace MiniJira.Presentation.UserInterfaceServices;

public class MainInterfaceWorker : IHostedService
{
    private readonly IUserInterfaceActionsService _userInterfaceActionsService;
    private readonly ITaskInterfaceActionsService _taskInterfaceActionsService;
    private readonly IAuthenticationService _authenticationService;

    public MainInterfaceWorker(
        IUserInterfaceActionsService userInterfaceActionsService,
        ITaskInterfaceActionsService taskInterfaceActionsService,
        IAuthenticationService authenticationService)
    {
        _userInterfaceActionsService = userInterfaceActionsService;
        _taskInterfaceActionsService = taskInterfaceActionsService;
        _authenticationService = authenticationService;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var user = await DoAuthenticateWhileHaveException(cancellationToken);
        Console.WriteLine($"Добрый день {user.Firstname} {user.Midname}");
        
        var availableActions = AvailableActionsHelper.GetAvailableActionsByRole(user.Role);

        int[] availableOptionsNumbers = [..availableActions.Keys, 0];
        while (true)
        {
            Console.WriteLine("Введите номер одной из опций:");
            foreach (var action in availableActions)
            {
                Console.WriteLine($"{action.Key}: {action.Value.Description}");
            }
            Console.WriteLine("Для выхода введите 0");
            var optionNumber = InputHelper.ReadInputIntFromList("Введите номер одной из опций:", availableOptionsNumbers.ToArray());
            if (optionNumber == 0)
            {
                return;
            }
            
            var neededAction = availableActions[optionNumber];
            await DoWhileHaveException(neededAction, user, cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task<UserEntity> DoAuthenticateWhileHaveException(CancellationToken cancellationToken)
    {
        while (true)
        {
            try
            {
                var user = await _authenticationService.AuthenticateUser(cancellationToken);
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}. \nПовторите заново.");
            }
        }
    }
    
    private async Task DoWhileHaveException(ActionInfo actionInfo, UserEntity user, CancellationToken cancellationToken)
    {
        while (true)
        {
            try
            {
                Task? task = null;
                if (actionInfo.ActionType == ActionType.UserActions)
                {
                    task = (Task)actionInfo.MethodInfo.Invoke(
                        _userInterfaceActionsService, 
                        actionInfo.NeedUserInfo ? [user, cancellationToken] : [cancellationToken])!;
                }
            
                if (actionInfo.ActionType == ActionType.TaskActions)
                {
                    task = (Task)actionInfo.MethodInfo.Invoke(
                        _taskInterfaceActionsService, 
                        actionInfo.NeedUserInfo ? [user, cancellationToken] : [cancellationToken])!;
                }

                if (task == null)
                {
                    return;
                }
                
                await task;
                
                Console.WriteLine("\nДля продолжения нажмите \"Enter\" ...");
                Console.ReadLine();
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}. \nПовторите заново.");
            }
        }
    }
}
