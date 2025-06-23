namespace MiniJira.Domain.Entities;

public class UserShortInfoEntity
{
    public int Id { get; set; }
    public string Login { get; set; }
    public string FirstName { get; set; }
    public string MidName { get; set; }
    public string LastName { get; set; }
}