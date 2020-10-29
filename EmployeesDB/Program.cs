using EmployeesDB.Data.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace EmployeesDB
{
    class Program
    {
        static private EmployeesContext _context = new EmployeesContext();
        static void Main(string[] args)
        {
            DeleteTown();
        }
        static string GetEmployeesInformation()
        {
            var employees = _context.Employees
            .OrderBy(e => e.EmployeeId)
            .Select(e => new
            {
                e.FirstName,
                e.LastName,
                e.MiddleName,
                e.JobTitle
            })
            .ToList();
            var sb = new StringBuilder();
            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle}");
            }
            return sb.ToString().TrimEnd();
        }
        static string EmployeesWithSalary()
        {
            var employees = _context.Employees
            .Where(e => e.Salary >= 48000)
            .OrderBy(e => e.LastName)
            .ToList();

            var sb = new StringBuilder();
            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary}");
            }
            return sb.ToString().TrimEnd();
        }
  
        private static void SmallDemartments()
        {
            var departments = _context.Departments
                .Where(e => e.Employees.Count < 5)
                .Select(e => e.Name)
                .ToList();
            Console.WriteLine(string.Join(", " , departments));
        }

        private static void Browns()
        {
            var newTown = new Towns
            {
                Name = "Los Angeles"
            };
            _context.Towns.Add(newTown);
            _context.SaveChanges();

            var address = new Addresses
            {
                AddressText = "Montage Beverly Hills225 N Canon Dr, Beverly Hills, CA 90210",
                Town = newTown
            };
            _context.Addresses.Add(address);
            _context.SaveChanges();

            var employees = _context.Employees
                .Where(e => e.LastName == "Brown")
                .ToList();
            employees.ForEach(em => em.Address = address);
            _context.SaveChanges();
        }

        private static void FindEbployeeById()
        {
            int id = int.Parse(Console.ReadLine());
            var employee = _context.Employees.Find(id);
            var projects = _context.EmployeesProjects
                .Where(e => e.EmployeeId == id)
                .Select(e => e.Project)
                .ToList();
            if (employee != null)
            {
                Console.WriteLine($"{employee.LastName} {employee.FirstName} {employee.MiddleName} - {employee.JobTitle}");
                foreach (var project in projects)
                {
                    Console.WriteLine($"{project.Name}");
                }
            }
        }

        private static void UpdatingSalary()
        {
            string d = Console.ReadLine();
            int p = int.Parse(Console.ReadLine());
            var department = _context.Departments
                .First(e => e.Name == d);
            foreach(var employee in department.Employees)
            {
                employee.Salary *= ( 100 + p / 100);
            }
            _context.SaveChanges();
        }
        //????
        private static void DeleteTown()
        {
            string deletetown = Console.ReadLine();
            var town = _context.Towns
                .First(t => t.Name == deletetown);
            _context.Towns.Remove(town);
            _context.SaveChanges();
        }

        private static void ProjectsAudit()
        {
            
        }
    }
}
