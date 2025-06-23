using System;
using MiniJira.Domain.Entities;
using MiniJira.Domain.Enums;

namespace MiniJira.Infrastructure.Users;

public static class UserConverter
{
    public static string ToDbRole(this UserRoles role)
    {
        return role switch
        {
            UserRoles.Customer => UserRolesDbConsts.Customer,
            UserRoles.Manager => UserRolesDbConsts.Manager,
            _ => throw new ArgumentOutOfRangeException(nameof(role), role, null)
        };
    }

    public static UserEntity ToUserEntity(this UserDb user)
    {
        return new UserEntity
        {
            Id = user.Id,
            Login = user.Login,
            Password = user.Password,
            Role = user.Role.ToDomainRole(),
            FirstName = user.FirstName,
            LastName = user.LastName,
            MidName = user.MidName
        };
    }
    
    private static UserRoles ToDomainRole(this string role)
    {
        return role switch
        {
            UserRolesDbConsts.Customer => UserRoles.Customer,
            UserRolesDbConsts.Manager => UserRoles.Manager,
            _ => throw new ArgumentOutOfRangeException(nameof(role), role, null)
        };
    }
}