using MiniJira.Domain.Enums;

namespace MiniJira.DomainServices.Converters;

public static class EnumConverter
{
    public static string ToRuString(this UserRoles role)
    {
        return role switch
        {
            UserRoles.Customer => "Сотрудник",
            UserRoles.Manager => "Менеджер",
            _ => throw new ArgumentOutOfRangeException(nameof(role), role, null)
        };
    }
    
    public static string ToRuString(this TaskStatuses status)
    {
        return status switch
        {
            TaskStatuses.ToDo => "К выполнению",
            TaskStatuses.InProgress => "В процессе",
            TaskStatuses.Done => "Выполнено",
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };
    }
}