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
    public class DepartmentController: ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public DepartmentController(ApplicationDbContext db)
        {
            _db = db;
        }

        //Action methods
        [HttpGet]
        public IActionResult GetDepartments()
        {
            return Ok(_db.Departments.ToList());
        }

       [HttpPost("{id}")]
       public async Task<IActionResult> AddDepartment([FromRoute] int id, [FromBody] Department objDepart)
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult("Error while adding the department");
            }
            else
            {
                var deptId = _db.Departments.Where(x => x.DepartmentName == objDepart.DepartmentName).Select(x => x.DepartmentId).ToList();
                var employees = _db.Employees.Where(x => deptId[0] == objDepart.DepartmentId).Select(x => x.firstName).ToList();
                string allEmployees = string.Join(",", employees);
                objDepart.Personnel = allEmployees;
                _db.Departments.Add(objDepart);
                await _db.SaveChangesAsync();

                return new JsonResult("Department added successfully");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment([FromRoute] int id, [FromBody] Department objDepart)
        {
            if(objDepart == null || id != objDepart.DepartmentId)
            {
                return new JsonResult("Department does not exists");
            }
            else
            {
                _db.Departments.Update(objDepart);
                await _db.SaveChangesAsync();

                return new JsonResult("Department updated successfully");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment([FromRoute] int id)
        {
            var findDepart = await _db.Departments.FindAsync(id);
            if(findDepart == null)
            {
                return NotFound();
            }
            else
            {
                _db.Departments.Remove(findDepart);
                await _db.SaveChangesAsync();

                return new JsonResult("Department deleted successfully!");
            }
        }
    }
}
