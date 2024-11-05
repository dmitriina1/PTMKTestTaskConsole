using PTMKTestTaskConsole.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using PTMKTestTaskConsole.DataAccess;

namespace PTMKTestTaskConsole.BusinessLogic
{
    public class EmployeeService
    {
        public void CreateEmployeeTable()
        {
            using (var context = new DatabaseContext())
            {
                context.CreateTable();
            }
        }

        public void AddEmployee(string fullName, DateOnly birthDate, string gender)
        {
            var employee = new Employee
            {
                FullName = fullName,
                BirthDate = birthDate,
                Gender = gender
            };
            employee.SaveToDatabase();
        }

        public void ListEmployees()
        {
            using (var context = new DatabaseContext())
            {
                var employees = context.GetAllEmployees();
                foreach (var employee in employees)
                {
                    Console.WriteLine($"{employee.FullName}, {employee.BirthDate.ToShortDateString()}, {employee.Gender}, Age: {employee.CalculateAge()}");
                }
            }
        }

        public void BulkInsertEmployees(int count)
        {
            var random = new Random();
            var employees = new List<Employee>();

            for (int i = 0; i < 100; i++)
            {
                var employee = new Employee
                {
                    FullName = $"F{GenerateRandomName(random)} {GenerateRandomFirstName(random)}",
                    BirthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-random.Next(18, 65))),
                    Gender = "Male"
                };
                employees.Add(employee);
            }

            for (int i = 0; i < count - 100; i++)
            {
                var gender = (i % 2 == 0) ? "Male" : "Female";
                var employee = new Employee
                {
                    FullName = $"{GenerateRandomInitial(random)}{GenerateRandomName(random)} {GenerateRandomFirstName(random)}",
                    BirthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-random.Next(18, 65))),
                    Gender = gender
                };
                employees.Add(employee);
            }

            using (var context = new DatabaseContext())
            {
                context.BulkInsertEmployees(employees);
            }

            Console.WriteLine($"Заполнение базы данных {count} записями завершено.");
        }

        private string GenerateRandomName(Random random)
        {
            const string letters = "abcdefghijklmnopqrstuvwxyz";
            var name = new char[random.Next(5, 9)];
            for (int i = 0; i < name.Length; i++)
            {
                name[i] = letters[random.Next(letters.Length)];
            }
            return Capitalize(name);
        }

        private string GenerateRandomFirstName(Random random)
        {
            string[] firstNames = { "John", "Jane", "Alex", "Chris", "Pat", "Sam", "Taylor", "Jordan", "Morgan", "Casey", "Leonardo", "Donatello" };
            return firstNames[random.Next(firstNames.Length)];
        }

        private char GenerateRandomInitial(Random random)
        {
            const string initials = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return initials[random.Next(initials.Length)];
        }

        private string Capitalize(char[] name)
        {
            name[0] = char.ToUpper(name[0]);
            return new string(name);
        }

        public void ListEmployeesByCriteria()
        {
            using (var context = new DatabaseContext())
            {
                var stopwatch = Stopwatch.StartNew();

                var employees = context.GetEmployeesByCriteria();
                foreach (var employee in employees)
                {
                    Console.WriteLine($"{employee.FullName}, {employee.BirthDate.ToShortDateString()}, {employee.Gender}, Age: {employee.CalculateAge()}");
                }
                stopwatch.Stop();
                Console.WriteLine($"Время выполнения: {stopwatch.ElapsedMilliseconds} миллисекунд.");
            }
        }
    }
}

