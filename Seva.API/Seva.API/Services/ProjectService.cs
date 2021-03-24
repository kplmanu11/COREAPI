using Seva.API.Infrastructure;
using Seva.API.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seva.API.Services
{
    public interface IProjectService
    {
        ICollection<Project> GetAllProject();
        Project GetProject(int id);
        Task<int> AddUpdateProject(Project Project);
        Task<int> Delete(int id);
    }

    public class ProjectService : IProjectService
    {
        protected readonly AppDbContext _dbContext;
        public ProjectService()
        {

        }

        public ICollection<Project> GetAllProject()
        {
            return (from r in _dbContext.Projects select r).ToList();
        }

        public Project GetProject(int id)
        {
            return _dbContext.Projects.Find(id);
        }

        public async Task<int> AddUpdateProject(Project Project)
        {
            if (Project.ID > 0)
            {
                _dbContext.Projects.Update(Project);
            }
            else
            {
                _dbContext.Projects.Add(Project);
            }
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> Delete(int id)
        {
            var emp = _dbContext.Projects.Find(id);
            if (!(emp is null))
            {
                emp.IsDeleted = true;
            }
            else
            {
                throw new Exception("Invalid or wrong Id");
            }
            _dbContext.Projects.Update(emp);
            return await _dbContext.SaveChangesAsync();
        }

    }
}
