using Seva.API.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Seva.API.Services
{
    public interface IEmployeeService
    {
        ICollection<Employee> GetAllEmployee();
        Employee GetEmployee(int id);
        Task<int> AddUpdateEmployee(Employee employee);
        Task<int> Delete(int id);
    }

    public class EmployeeService: IEmployeeService
    {
        protected readonly AppDbContext _dbContext;
        public EmployeeService()
        {
            
        }

        public ICollection<Employee> GetAllEmployee()
        {
            return (from r in _dbContext.Employees select r).ToList();
        }

        public Employee GetEmployee( int id)
        {
            return _dbContext.Employees.Find(id);
        }

        public async Task<int> AddUpdateEmployee(Employee employee)
        {
            if(employee.ID > 0)
            {
                _dbContext.Employees.Update(employee);
            }
            else
            {
                _dbContext.Employees.Add(employee);
            }            
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> Delete(int id)
        {
            var emp = _dbContext.Employees.Find(id);
            if(!(emp is null))
            {
                emp.IsDeleted = true;
            }
            else
            {
                throw new Exception("Invalid or wrong Id");
            }
            _dbContext.Employees.Update(emp);
            return await _dbContext.SaveChangesAsync();
        }

    }
}
