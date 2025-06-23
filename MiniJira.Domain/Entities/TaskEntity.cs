using System;
using MiniJira.Domain.Enums;

namespace MiniJira.Domain.Entities
{
    public class TaskEntity
    {
        public long Id { get; set; }
        public int ProjectNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TaskStatuses Status { get; set; }
        public string Manager { get; set; }
        public string Customer { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
    }
}