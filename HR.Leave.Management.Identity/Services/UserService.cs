using HR.Leave.Management.Identity.Models;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HR.Leave.Management.Identity.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;

        public UserService(UserManager<ApplicationUser> userManager, IHttpContextAccessor contextAccessor)
        {
            _userManager = userManager;
            _contextAccessor = contextAccessor;
        }

        public string UserId { get => _contextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier); }

        public async Task<Employee> GetEmployee(string userId)
        {
            var employee = await _userManager.FindByIdAsync(userId);

            return new Employee
            {
                Email = employee.Email,
                Id = employee.Id,
                Firstname = employee.FirstName,
                Lastname = employee.LastName,
            };
        }

        public async Task<List<Employee>> GetEmployees()
        {
            var employees = await _userManager.GetUsersInRoleAsync("Employee");

            return employees.Select(emp => new Employee
            {
                Id = emp.Id,
                Email = emp.Email,
                Firstname = emp.FirstName,
                Lastname = emp.LastName,
            }).ToList();
        }
    }
}
