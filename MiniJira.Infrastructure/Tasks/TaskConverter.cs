using MiniJira.Domain.Entities;
using MiniJira.Domain.Enums;

namespace MiniJira.Infrastructure.Tasks;

public static class TaskConverter
{
    public static string ToDbStatus(this TaskStatuses status)
    {
        return status switch
        {
            TaskStatuses.ToDo => TaskStatusesDbConsts.ToDo,
            TaskStatuses.InProgress => TaskStatusesDbConsts.InProgress,
            TaskStatuses.Done => TaskStatusesDbConsts.Done,
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };
    }

    public static TaskEntity ToTaskEntity(this TaskDb task)
    {
        return new TaskEntity
        {
            Id = task.Id,
            ProjectNumber = task.ProjectNumber,
            Name = task.Name,
            Description = task.Description,
            Status = task.Status.ToDomainStatus(),
            Manager = task.Manager,
            CreatedAt = task.CreatedAt,
            LastUpdatedAt = task.LastUpdatedAt
        };
    }

    private static TaskStatuses ToDomainStatus(this string status)
    {
        return status switch
        {
            TaskStatusesDbConsts.ToDo => TaskStatuses.ToDo,
            TaskStatusesDbConsts.InProgress => TaskStatuses.InProgress,
            TaskStatusesDbConsts.Done => TaskStatuses.Done,
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };
    }
}
