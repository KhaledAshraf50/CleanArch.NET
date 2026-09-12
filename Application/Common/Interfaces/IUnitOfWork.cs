using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<Project> Projects { get; }
        IGenericRepository<Domain.Entities.Task> Tasks { get; }
        IGenericRepository<Comment> Comments { get; }

        Task<int> SaveChangesAsync();
    }
}
