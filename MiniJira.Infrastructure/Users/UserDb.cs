namespace MiniJira.Infrastructure.Users;

public class UserDb
{
    public int Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public string Firstname { get; set; }
    public string Midname { get; set; }
    public string Lastname { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastChangePasswordDate { get; set; }
}