using System.Reflection;
using MiniJira.Presentation.Enums;

namespace MiniJira.Presentation.Contracts;

public class ActionInfo
{
    public ActionType ActionType { get; set; }
    public MethodInfo MethodInfo { get; set; }
    public string Description { get; set; }
    public bool NeedUserInfo { get; set; }
}