using System.Reflection;
using MiniJira.Domain.Entities;
using MiniJira.Domain.Enums;
using MiniJira.Presentation.Contracts;
using MiniJira.Presentation.CustomAttributes;
using MiniJira.Presentation.Enums;
using MiniJira.Presentation.Interfaces;

namespace MiniJira.Presentation.Helpers;

public static class AvailableActionsHelper
{
    public static Dictionary<int, ActionInfo> GetAvailableActionsByRole(UserRoles role)
    {
        var userAction = GetAvailableUserActionsByRole(role);
        var taskAction = GetAvailableTaskActionsByRole(role);
        var availableActionsByActionNumber = new Dictionary<int, ActionInfo>();
        var numberAction = 1;
        foreach (var action in userAction)
        {
            availableActionsByActionNumber.Add(numberAction, new ActionInfo
            {
                ActionType = ActionType.UserActions,
                Description = action.Description,
                MethodInfo = action.MethodInfo,
                NeedUserInfo = action.MethodInfo.GetParameters().FirstOrDefault(p => p.ParameterType == typeof(UserEntity)) != null
            });
            numberAction++;
        }
        foreach (var action in taskAction)
        {
            availableActionsByActionNumber.Add(numberAction, new ActionInfo
            {
                ActionType = ActionType.TaskActions,
                Description = action.Description,
                MethodInfo = action.MethodInfo,
                NeedUserInfo = action.MethodInfo.GetParameters().FirstOrDefault(p => p.ParameterType == typeof(UserEntity)) != null
            });
            numberAction++;
        }
        return availableActionsByActionNumber;
    }
    
    private static (MethodInfo MethodInfo, string Description)[] GetAvailableUserActionsByRole(UserRoles role)
    {
        return GetAvailableActionsByRole<IUserInterfaceActionsService>(role);
    }
    
    private static (MethodInfo MethodInfo, string Description)[] GetAvailableTaskActionsByRole(UserRoles role)
    {
        return GetAvailableActionsByRole<ITaskInterfaceActionsService>(role);
    }
    
    private static (MethodInfo MethodInfo, string Description)[] GetAvailableActionsByRole<T>(UserRoles role)
        where T : class
    {
        var methodInfosWithDescriptions = new List<(MethodInfo MethodInfo, string Description, int SortNumber)>();
        var methodInfos = typeof(T).GetMethods();
        foreach (var methodInfo in methodInfos)
        {
            var accessByRoleAttribute = methodInfo.GetCustomAttribute<AccessByRoleAttribute>();
            if (accessByRoleAttribute == null || !accessByRoleAttribute.AllowedRoles.Contains(role))
            {
                continue;
            }
            
            methodInfosWithDescriptions.Add((
                methodInfo, 
                accessByRoleAttribute.FunctionalDescription,
                accessByRoleAttribute.SortNumber));
        }
        
        return methodInfosWithDescriptions
            .OrderBy(i => i.SortNumber)
            .Select(i => (i.MethodInfo, i.Description))
            .ToArray();
    }
}