using Microsoft.EntityFrameworkCore;
using WebApi.Crud.Models;

namespace WebApi.Crud.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
              
        }

        //creating DbSet to enable ef migrations for Employee table
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
    }
}
