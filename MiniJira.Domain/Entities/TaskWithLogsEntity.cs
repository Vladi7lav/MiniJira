namespace MiniJira.Domain.Entities;

public class TaskWithLogsEntity
{
    public TaskEntity? Task { get; set; }
    public LogEntity[] Logs { get; set; }
}