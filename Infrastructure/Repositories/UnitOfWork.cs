using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Task = Domain.Entities.Task;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IGenericRepository<Task> Tasks { get; }
        public IGenericRepository<Comment> Comments { get; }
        public IGenericRepository<Project> Projects { get; }


        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Projects = new GenericRepository<Project>(context);
            Tasks = new GenericRepository<Task>(_context);
            Comments = new GenericRepository<Comment>(_context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
