using MiniJira.Domain.Enums;

namespace MiniJira.Presentation.CustomAttributes;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class AccessByRoleAttribute : Attribute
{
    public AccessByRoleAttribute(UserRoles[] roles, string functionalDescription, int sortNumber)
    {
        AllowedRoles = roles;
        FunctionalDescription = functionalDescription;
        SortNumber = sortNumber;
    }
    
    public AccessByRoleAttribute(UserRoles role, string functionalDescription, int sortNumber)
    {
        AllowedRoles = [role];
        FunctionalDescription = functionalDescription;
        SortNumber = sortNumber;
    }
    
    public UserRoles[] AllowedRoles { get; set; }
    
    public string FunctionalDescription { get; set; }
    
    public int SortNumber { get; set; }
}