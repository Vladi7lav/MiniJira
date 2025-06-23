using MiniJira.Domain.Entities;

namespace MiniJira.DomainServices.Services.Users.Contracts;

public class LoginResult
{
    ct

    public bool Success { get; set; }
    public string? Description { get; set; }
    public UserEntity? User { get; set; }
}
