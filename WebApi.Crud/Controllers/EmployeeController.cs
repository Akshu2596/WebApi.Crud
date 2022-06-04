using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Crud.Data;
using WebApi.Crud.Models;

namespace WebApi.Crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        public readonly ApplicationDbContext _db;
        public EmployeeController(ApplicationDbContext db)
        {
            _db = db;
        }

        //Defining Action methods for the API

        [HttpGet]
        public IActionResult GetEmployees()
        {
            return Ok(_db.Employees.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] Employee objEmployee)
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult("Error while adding a new employee");
            }
            _db.Employees.Add(objEmployee);
            await _db.SaveChangesAsync();

            return new JsonResult("Employee added successfully");

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee([FromRoute] int id, [FromBody] Employee objEmployee)
        {
            if (objEmployee == null || id != objEmployee.id)
            {
                return new JsonResult("Employee does not exist!");
            }
            else
            {
                var currentDepartment = _db.Employees.Where(x => x.firstName == objEmployee.firstName).Select(x => x.Department).ToString();
                string employee = _db.Employees.Where(x => x.Department == currentDepartment).Select(x => x.firstName).ToString();
                Department data = new Department()
                {
                    Personnel = employee
                };
                TempData["personnelData"] = data;
                if (objEmployee.Department != currentDepartment)
                {
                    _db.Departments.Remove(data);
                    await _db.SaveChangesAsync(); 
                }
                _db.Employees.Update(objEmployee);
                await _db.SaveChangesAsync();

                return new JsonResult("Employee data updated successfully");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] int id)
        {
            var findEmp = await _db.Employees.FindAsync(id);
            if (findEmp == null)
            {
                return NotFound();
            }
            else
            {
                _db.Employees.Remove(findEmp);
                await _db.SaveChangesAsync();
                return new JsonResult("Employee Deleted successfully");
            }
        }        
    }
}
