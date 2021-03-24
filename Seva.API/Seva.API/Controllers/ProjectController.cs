using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Seva.API.Infrastructure;
using Seva.API.Infrastructure.Entities;
using Seva.API.Services;

namespace Seva.API.Controllers
{
    public class ProjectController : BaseController
    {
        private readonly IProjectService _projectService;
        public ProjectController(UserManager<LoginUser> userManager, IProjectService projectService) : base(userManager)
        {
            projectService = _projectService;
        }

        // GET: api/<EmployeeController>
        [HttpGet]
        public IEnumerable<Project> Get()
        {
            return _projectService.GetAllProject();
        }

        // GET api/<EmployeeController>/5
        [HttpGet("{id}")]
        public Project Get(int id)
        {
            return _projectService.GetProject(id);
        }

        // POST api/<EmployeeController>
        [HttpPost]
        public async Task<int> Post([FromBody] Project project)
        {
            return await _projectService.AddUpdateProject(project);

        }

        // PUT api/<EmployeeController>/5
        [HttpPut("{id}")]
        public async Task<int> Put(int id, [FromBody] Project project)
        {
            project.ID = id;
            return await _projectService.AddUpdateProject(project);

        }

        // DELETE api/<EmployeeController>/5
        [HttpDelete("{id}")]
        public async Task<int> Delete(int id)
        {
            return await _projectService.Delete(id);
        }
    }
}
