namespace MiniJira.Domain.Entities;

public class UserBaseInfoEntity
{
    public int Id { get; set; }
    public string Login { get; set; }
    public string Firstname { get; set; }
    public string Midname { get; set; }
    public string Lastname { get; set; }
    
    public string GetFullname() => $"{Lastname} {Firstname} {Midname}";
}