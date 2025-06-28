namespace MiniJira.Presentation.Helpers;

public static class TaskInterfaceHelper
{
    public static long GetTaskId()
        => InputHelper.ReadInputLong("Введите Id задачи: ");
    public static string GetTaskName(bool isUpdate = false)
        => InputHelper.ReadInputString($"Введите {(isUpdate ? "новое" : string.Empty)} название задачи: ");
    public static int GetProjectNumber(bool isUpdate = false)
        => InputHelper.ReadInputInt($"Введите {(isUpdate ? "новый" : string.Empty)} номер проекта: ");
    public static string GetDescription() 
        => InputHelper.ReadInputString("Введите описание задачи: ");
    public static string GetManager() 
        => InputHelper.ReadInputString("Введите логин менедежера: ");
    public static string GetCustomer() 
        => InputHelper.ReadInputString("Введите логин исполнителя: ");
}