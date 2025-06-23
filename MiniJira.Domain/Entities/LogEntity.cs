using System;

namespace MiniJira.Domain.Entities
{
    public class LogEntity
    {
        public int TaskId { get; set; }
        public string Description { get; set; }
        public string UserNameWhoChanged { get; set; }
        public DateTime ActionDate { get; set; }
    }
}