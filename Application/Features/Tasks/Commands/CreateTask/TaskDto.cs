using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Tasks.Commands.CreateTask
{
    public class TaskDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        public string Status { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        public Guid ProjectId { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
