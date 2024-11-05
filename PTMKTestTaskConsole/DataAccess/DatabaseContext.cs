using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Globalization;
using PTMKTestTaskConsole.Model;

namespace PTMKTestTaskConsole.DataAccess
{
    internal class DatabaseContext : IDisposable
    {
        private readonly SQLiteConnection _connection;

        public DatabaseContext()
        {
            _connection = new SQLiteConnection("Data Source=employee.db;Version=3;");
            _connection.Open();
        }

        public void CreateTable()
        {
            string query = @"
                CREATE TABLE IF NOT EXISTS Employees (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    FullName TEXT NOT NULL,
                    BirthDate DATE NOT NULL,
                    Gender TEXT NOT NULL
                );";
            using (var command = new SQLiteCommand(query, _connection))
            {
                command.ExecuteNonQuery();
            }
        }

        public void InsertEmployee(Employee employee)
        {
            string query = "INSERT INTO Employees (FullName, BirthDate, Gender) VALUES (@FullName, @BirthDate, @Gender)";
            using (var command = new SQLiteCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@FullName", employee.FullName);
                command.Parameters.AddWithValue("@BirthDate", employee.BirthDate.ToDateTime(new TimeOnly(0, 0))); 
                command.Parameters.AddWithValue("@Gender", employee.Gender);
                command.ExecuteNonQuery();
            }
        }

        public void BulkInsertEmployees(List<Employee> employees)
        {
            using (var transaction = _connection.BeginTransaction())
            {
                foreach (var employee in employees)
                {
                    InsertEmployee(employee);
                }
                transaction.Commit();
            }
        }

        public List<Employee> GetAllEmployees()
        {
            List<Employee> employees = new List<Employee>();
            string query = "SELECT * FROM Employees ORDER BY FullName";
            using (var command = new SQLiteCommand(query, _connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    employees.Add(new Employee
                    {
                        Id = reader.GetInt32(0),
                        FullName = reader.GetString(1),
                        BirthDate = DateOnly.FromDateTime(reader.GetDateTime(2)),
                        Gender = reader.GetString(3)
                    });
                }
            }
            return employees;
        }

        public List<Employee> GetEmployeesByCriteria()
        {
            List<Employee> employees = new List<Employee>();
            string query = "SELECT * FROM Employees WHERE Gender = 'Male' AND FullName LIKE 'F%'";

            using (var command = new SQLiteCommand(query, _connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    employees.Add(new Employee
                    {
                        Id = reader.GetInt32(0),
                        FullName = reader.GetString(1),
                        BirthDate = DateOnly.FromDateTime(reader.GetDateTime(2))    ,
                        Gender = reader.GetString(3)
                    });
                }
            }
            return employees;
        }

        public bool DatabaseExists()
        {
            string query = "SELECT name FROM sqlite_master WHERE type='table' AND name='Employees';";
            using (var command = new SQLiteCommand(query, _connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    return reader.HasRows; 
                }
            }
        }

        public void Dispose()
        {
            _connection?.Close();
        }
    }
}

