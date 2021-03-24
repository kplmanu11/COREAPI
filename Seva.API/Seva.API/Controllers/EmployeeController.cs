using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Seva.API.Infrastructure;
using Seva.API.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Seva.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : BaseController
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeController(UserManager<LoginUser> userManager, IEmployeeService employeeService) : base(userManager)
        {
            _employeeService = employeeService;
        }

        // GET: api/<EmployeeController>
        [HttpGet]
        public IEnumerable<Employee> Get()
        {
            return _employeeService.GetAllEmployee();
        }

        // GET api/<EmployeeController>/5
        [HttpGet("{id}")]
        public Employee Get(int id)
        {
            return _employeeService.GetEmployee(id);
        }

        // POST api/<EmployeeController>
        [HttpPost]
        public async Task<int> Post([FromBody] Employee employee)
        {
          return await _employeeService.AddUpdateEmployee(employee);
            
        }

        // PUT api/<EmployeeController>/5
        [HttpPut("{id}")]
        public async Task<int> Put(int id, [FromBody] Employee employee)
        {
            employee.ID = id;
            return await _employeeService.AddUpdateEmployee(employee);

        }

        // DELETE api/<EmployeeController>/5
        [HttpDelete("{id}")]
        public async Task<int> Delete(int id)
        {
            return await _employeeService.Delete(id);
        }
    }
}
