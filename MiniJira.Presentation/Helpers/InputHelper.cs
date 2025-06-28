using System.Text;
using MiniJira.Domain.Enums;
using MiniJira.DomainServices.Converters;

namespace MiniJira.Presentation.Helpers;

public static class InputHelper
{
    public static string ReadInputString(string messageForUser)
    {
        Console.WriteLine(messageForUser);
        while (true)
        {
            var input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
            {
                return input;
            }
            Console.WriteLine("Вы ничего не ввели, повторите ввод:");
        }
    }
    
    public static int ReadInputInt(string messageForUser)
    {
        Console.WriteLine(messageForUser);
        while (true)
        {
            if (int.TryParse(Console.ReadLine()?.Replace(" ", ""), out var inputInt))
            {
                return inputInt;
            }
            Console.WriteLine("Не удалось преобразовать введённые данные в число, повторите ввод:");
        }
    }
    
    public static int? ReadInputNullableInt(string messageForUser)
    {
        Console.WriteLine(messageForUser);
        while (true)
        {
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }
            if (int.TryParse(input.Replace(" ", ""), out var inputInt))
            {
                return inputInt;
            }
            Console.WriteLine("Не удалось преобразовать введённые данные в число, повторите ввод:");
        }
    }
    
    
    public static int ReadInputIntFromList(string messageForUser, int[] availableValues)
    {
        while (true)
        {
            var input = ReadInputInt(messageForUser);
            if (availableValues.Contains(input))
            {
                return input;
            }
        }
    }
    
    public static int? ReadInputIntFromListOrNull(string messageForUser, int[] availableValues)
    {
        while (true)
        {
            var input = ReadInputNullableInt(messageForUser);
            if (input == null)
            {
                return null;
            }
            if (availableValues.Contains(input.Value))
            {
                return input;
            }
        }
    }

    public static UserRoles ReadUserRole(string messageForUser = "Введите номер роли пользователя: ")
    {
        var userRoles = Enum.GetValues(typeof(UserRoles));
        var sb = new StringBuilder(messageForUser);
        foreach (UserRoles role in userRoles)
        {
            sb.AppendLine($"{(int)role} - {role.ToRuString()}");
        }
        var fullMessage = sb.ToString();
        var userRolesIntVariants = userRoles.Cast<int>().ToArray();
        return (UserRoles)ReadInputIntFromList(fullMessage,  userRolesIntVariants);
    }
    
    public static TaskStatuses? ReadTaskStatus(string messageForUser = "Введите номер статуса задачи: ", bool nullable = false)
    {
        var taskStatuses = Enum.GetValues(typeof(UserRoles));
        var sb = new StringBuilder(messageForUser);
        foreach (TaskStatuses status in taskStatuses)
        {
            sb.AppendLine($"{(int)status} - {status.ToRuString()}");
        }
        var fullMessage = sb.ToString();
        var taskStatusesIntVariants = taskStatuses.Cast<int>().ToArray();
        return nullable 
            ? (TaskStatuses)ReadInputIntFromList(fullMessage, taskStatusesIntVariants)
            : (TaskStatuses?)ReadInputIntFromListOrNull(fullMessage, taskStatusesIntVariants);
    }
    
    public static long ReadInputLong(string messageForUser)
    {
        Console.WriteLine(messageForUser);
        while (true)
        {
            if (long.TryParse(Console.ReadLine()?.Replace(" ", ""), out var inputLong))
            {
                return inputLong;
            }
            Console.WriteLine("Не удалось преобразовать введённые данные в число, повторите ввод:");
        }
    }
}