namespace MiniJira.DomainServices.SharedContracts;

public class SimpleOperationResult
{
    public SimpleOperationResult(bool isSuccess)
    {
        Success = isSuccess;
    }

    public SimpleOperationResult(bool isSuccess, string message)
    {
        Success = isSuccess;
        ErrorMessage = message;
    }
    
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
}