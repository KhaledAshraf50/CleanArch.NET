using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Comment :BaseEntity
    {
        public string Content { get; set; } = string.Empty;
        // Foreign Key
        public Guid TaskId { get; set; }
        public Task? Task { get; set; }
    }
}
