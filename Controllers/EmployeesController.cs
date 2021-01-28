using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASPex1.Models;
using Microsoft.AspNetCore.Cors;

namespace ASPex1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("EmployeePolicy")]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeContext _context;

        public EmployeesController(EmployeeContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            return await _context.Employees
                .Select(x => EmployeeToDTO(x))
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(long id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return EmployeeToDTO(employee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(long id, Employee EmployeeDTO)
        {
            if (id != EmployeeDTO.Id)
            {
                return BadRequest();
            }

            _context.Entry(EmployeeDTO).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee(Employee EmployeeDTO)
        {
            var employee = new Employee
            {
                Name = EmployeeDTO.Name,
                Surname = EmployeeDTO.Surname,
                Position = EmployeeDTO.Position,
                Salary = EmployeeDTO.Salary
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetEmployee),
                new { id = employee.Id },
                EmployeeToDTO(employee));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Employee>> DeleteEmployee(long id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetEmployee),
                new { id = employee.Id },
                EmployeeToDTO(employee)); 
        }

        private bool EmployeeExists(long id) =>
             _context.Employees.Any(e => e.Id == id);

        private static Employee EmployeeToDTO(Employee employee) =>
            new Employee
            {
                Id = employee.Id,
                Name = employee.Name,
                Surname = employee.Surname,
                Position = employee.Position,
                Salary = employee.Salary
            };
    }
}
