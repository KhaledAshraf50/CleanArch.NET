using Domain.Common;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    //public class TaskStatus
    //{

    //}
    public class Task :BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        public Enums.TaskStatus Status { get; set; }=Enums.TaskStatus.Todo;
        public DateTime? DueDate { get; set; }
        // Foreign Key
        public Guid ProjectId { get; set; }
        public Project? Project { get; set; }

        // Owner (Identity user id)
        public string? OwnerId { get; set; }

        // Navigation Property
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    }
}
