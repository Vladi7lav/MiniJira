using Dapper;
using MiniJira.Domain.Entities;
using MiniJira.Domain.Enums;
using MiniJira.Repository;

namespace MiniJira.Infrastructure.Tasks;

public class TaskRepository : ITaskRepository
{
    public async Task CreateTask(TaskEntity task, int managerId, CancellationToken cancellationToken)
    {
        var command = new CommandDefinition(
            """
            INSERT INTO tasks
                (
                    project_number,
                    name,
                    description,
                    manager_id,
                    customer_id
                )
            SELECT
            	@projectNumber,
            	@name,
            	@description,
            	@managerId,
            	u.id
            FROM users AS u
            WHERE u.login = @customerLogin
            """,
            new
            {
                projectNumber = task.ProjectNumber,
                name = task.Name,
                description = task.Description,
                managerId = managerId,
                customerLogin = task.Customer
            },
            cancellationToken: cancellationToken);
        using var connection = await DatabaseConnectionHelper.GetOpenConnection(cancellationToken);
        await connection.ExecuteAsync(command);
    }

    public async Task UpdateTaskStatus(long taskId, TaskStatuses status, CancellationToken cancellationToken)
    {
        var command = new CommandDefinition(
            """
            UPDATE tasks
            SET status = @status
            WHERE task_id = @taskId
            """,
            new
            {
                taskId = taskId,
                status = status.ToDbStatus()
            },
            cancellationToken: cancellationToken);
        using var connection = await DatabaseConnectionHelper.GetOpenConnection(cancellationToken);
        await connection.ExecuteAsync(command);
    }

    public async Task UpdateTask(
        long taskId,
        int? projectNumber,
        string? name,
        string? description,
        int? managerId,
        int? customerId,
        CancellationToken cancellationToken)
    {
        if (projectNumber == null
            && string.IsNullOrWhiteSpace(name)
            && string.IsNullOrWhiteSpace(description)
            && managerId == null
            && customerId == null)
        {
            throw new Exception("For update task, must be provided projectNumber, description, managerId or customerId");
        }

        var queryParameters = new DynamicParameters();
        var setters = new List<string>();
        if (projectNumber != null)
        {
            setters.Add("project_number = @projectNumber");
            queryParameters.Add("projectNumber", projectNumber);
        }
        if (!string.IsNullOrWhiteSpace(description))
        {
            setters.Add("description = @description");
            queryParameters.Add("description", description);
        }
        if (managerId != null)
        {
            setters.Add("manager_id = @managerId");
            queryParameters.Add("managerId", managerId);
        }
        if (customerId != null)
        {
            setters.Add("customer_id = @customerId");
            queryParameters.Add("customerId", customerId);
        }

        var queryText = "UPDATE tasks SET " + string.Join(", ", setters) + " WHERE task_id = @taskId;";
        queryParameters.Add("taskId", taskId);

        var command = new CommandDefinition(
            queryText,
            queryParameters,
            cancellationToken: cancellationToken);
        using var connection = await DatabaseConnectionHelper.GetOpenConnection(cancellationToken);
        await connection.ExecuteAsync(command);
    }

    public async Task<TaskEntity[]> GetTasksByCustomerId(int customerId, TaskStatuses? status, CancellationToken cancellationToken)
    {
        var queryText =
            """
            SELECT
                t.id as Id,
                project_number as ProjectNumber,
                name,
                description,
                status,
                m.login as Manager,
                c.login as Customer,
                t.created_at as CreatedAt,
                t.last_updated_at as LastUpdatedAt
            FROM users as c
            INNER JOIN tasks as t on t.customer_id = c.id
            INNER JOIN users as m on m.id = t.manager_id
            WHERE c.id = @customerId
            """;
        var queryParameters = new DynamicParameters();
        queryParameters.Add("customerId", customerId);
        if (status != null)
        {
            queryText += " AND t.status = @status::task_statuses;";
            queryParameters.Add("status", status.Value.ToDbStatus());
        }

        var command = new CommandDefinition(queryText, queryParameters, cancellationToken: cancellationToken);
        using var connection = await DatabaseConnectionHelper.GetOpenConnection(cancellationToken);
        return (await connection.QueryAsync<TaskDb>(command)).Select(t => t.ToTaskEntity()).ToArray();
    }

    public async Task<TaskEntity[]> GetTasksByCustomerLogin(int customerLogin, TaskStatuses? status, CancellationToken cancellationToken)
    {
        var queryText =
            """
            SELECT
                t.id as Id,
                project_number as ProjectNumber,
                name,
                description,
                status,
                m.login as Manager,
                c.login as Customer,
                t.created_at as CreatedAt,
                t.last_updated_at as LastUpdatedAt
            FROM users as c
            INNER JOIN tasks as t on t.customer_id = c.id
            INNER JOIN users as m on m.id = t.manager_id
            WHERE c.login = @customerLogin
            """;
        var queryParameters = new DynamicParameters();
        queryParameters.Add("customerLogin", customerLogin);
        if (status != null)
        {
            queryText += " AND t.status = @status::task_statuses;";
            queryParameters.Add("status", status.Value.ToDbStatus());
        }

        var command = new CommandDefinition(queryText, queryParameters, cancellationToken: cancellationToken);
        using var connection = await DatabaseConnectionHelper.GetOpenConnection(cancellationToken);
        return (await connection.QueryAsync<TaskDb>(command)).Select(t => t.ToTaskEntity()).ToArray();
    }

    public async Task<TaskEntity[]> GetTasksByManagerId(int managerId, TaskStatuses? status, CancellationToken cancellationToken)
    {
        var queryText =
            """
            SELECT
                t.id as Id,
                project_number as ProjectNumber,
                name,
                description,
                status,
                m.login as Manager,
                c.login as Customer,
                t.created_at as CreatedAt,
                t.last_updated_at as LastUpdatedAt
            FROM users as m
            INNER JOIN tasks as t on t.manager_id = m.id
            INNER JOIN users as c on c.id = t.customer_id
            WHERE m.id = @managerId
            """;
        var queryParameters = new DynamicParameters();
        queryParameters.Add("managerId", managerId);
        if (status != null)
        {
            queryText += " AND t.status = @status::task_statuses;";
            queryParameters.Add("status", status.Value.ToDbStatus());
        }

        var command = new CommandDefinition(queryText, queryParameters, cancellationToken: cancellationToken);
        using var connection = await DatabaseConnectionHelper.GetOpenConnection(cancellationToken);
        return (await connection.QueryAsync<TaskDb>(command)).Select(t => t.ToTaskEntity()).ToArray();
    }

    public async Task<TaskEntity?> GetTaskById(int taskId, CancellationToken cancellationToken)
    {
        var command = new CommandDefinition(
            """
            SELECT
                t.id as Id,
                project_number as ProjectNumber,
                name,
                description,
                status,
                m.login as Manager,
                c.login as Customer,
                t.created_at as CreatedAt,
                t.last_updated_at as LastUpdatedAt
            FROM tasks as t
            inner join users as c on c.id = t.customer_id
            inner join users as m on m.id = t.manager_id
            WHERE id = @taskId;
            """,
            new
            {
                taskId = taskId,
            },
            cancellationToken: cancellationToken);
        using var connection = await DatabaseConnectionHelper.GetOpenConnection(cancellationToken);
        return (await connection.QueryFirstOrDefaultAsync<TaskDb>(command))?.ToTaskEntity();
    }

    public async Task DeleteTask(long taskId, CancellationToken cancellationToken)
    {
        var command = new CommandDefinition(
            """
            DELETE from tasks
            WHERE task_id = @taskId
            """,
            new { taskId = taskId },
            cancellationToken: cancellationToken);
        using var connection = await DatabaseConnectionHelper.GetOpenConnection(cancellationToken);
        await connection.ExecuteAsync(command);
    }
}
