using MiniJira.Domain.Enums;

namespace MiniJira.Domain.Entities;

public class UserEntity : UserBaseInfoEntity
{
    public string Password { get; set; }
    public UserRoles Role { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastChangePasswordDate { get; set; }
}
