using System.ComponentModel.DataAnnotations;

namespace WebApi.Crud.Models
{
    public class Employee
    {
        public Employee()
        {
        }
        public int id
        {
            get;
            set;
        }
        public string firstName
        {
            get;
            set;
        }
        public string lastName
        {
            get;
            set;
        }
        [MinLength(10)]
        public string phoneNumber
        {
            get;
            set;
        }
        [Required]
        public string Department
        {
            get;
            set;
        }
    }
}
