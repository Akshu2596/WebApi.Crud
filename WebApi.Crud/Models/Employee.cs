using System;
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
        public string phoneNumber
        {
            get;
            set;
        }
    }
}
