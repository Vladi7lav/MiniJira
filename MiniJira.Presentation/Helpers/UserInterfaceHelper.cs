namespace MiniJira.Presentation.Helpers;

public static class UserInterfaceHelper
{
    public static string GetLogin(bool isUpdate = false)
        => InputHelper.ReadInputString($"Введите {(isUpdate ? "новый" : string.Empty)} логин: ");
    public static string GetPassword(bool isUpdate = false)
        => InputHelper.ReadInputString($"Введите {(isUpdate ? "новый" : string.Empty)} пароль: ");
    public static string GetFirstName() 
        => InputHelper.ReadInputString("Введите имя пользователя: ");
    public static string GetMidName() 
        => InputHelper.ReadInputString("Введите отчество пользователя: ");
    public static string GetLastName() 
        => InputHelper.ReadInputString("Введите фамилию пользователя: ");
}