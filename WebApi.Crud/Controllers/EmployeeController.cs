using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                //todo: catch different types of model state errors
                return new JsonResult("Error while adding a new employee");
            }
            _db.Employees.Add(objEmployee);
            await _db.SaveChangesAsync();

            return new JsonResult("Employee added successfully");

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee([FromRoute] int id, [FromBody] Employee objEmployee)
        {
            //different types of request params
            // conventional & attribute routing
            if (objEmployee == null || id != objEmployee.id)
            {
                return new JsonResult("Employee does not exist!");
            }
            
            _db.Employees.Update(objEmployee);
            await _db.SaveChangesAsync();
            var empDepts = (_db.Departments.Where(x => x.Personnel.Contains(objEmployee.firstName))).ToList();
            if(empDepts != null && empDepts.Count >0) {
                foreach(Department objDept in empDepts)
                {
                    var employees = _db.Employees.Where(x => x.Department == objDept.DepartmentName).Select(x => x.firstName).ToList();
                    string allEmployees = string.Join(",", employees);
                    objDept.Personnel = allEmployees;
                    _db.Departments.Attach(objDept);
                    _db.Entry(objDept).State = EntityState.Modified;
                }
            }
            await _db.SaveChangesAsync();
            return new JsonResult("Employee data updated successfully");
            
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
