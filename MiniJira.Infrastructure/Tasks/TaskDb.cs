namespace MiniJira.Infrastructure.Tasks;

public class TaskDb
{
    public long Id { get; set; }
    public int ProjectNumber { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public string Manager { get; set; }
    public string Customer { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
}
