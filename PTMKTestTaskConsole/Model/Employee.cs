using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PTMKTestTaskConsole.DataAccess;

namespace PTMKTestTaskConsole.Model
{
    public class Employee
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateOnly BirthDate { get; set; }
        public string Gender { get; set; }

        public void SaveToDatabase()
        {
            using (var context = new DatabaseContext())
            {
                context.InsertEmployee(this);
            }
        }

        public int CalculateAge()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            int age = today.Year - BirthDate.Year;
            if (BirthDate > today.AddYears(-age)) age--;
            return age;
        }
    }
}
