using EmployeesDB.Data.Models;
using System;
using System.Collections.Generic;
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
            SmallDemartments();
        }
        static void EmployeesWithSalary()
        {
            IQueryable<Employees> employees = from employee in _context.Employees
                                              where employee.Salary > 48000
                                              orderby employee.LastName descending
                                              select employee;

            foreach (Employees emp in employees)
            {
                Console.WriteLine($"{emp.LastName} {emp.FirstName} {emp.MiddleName} - {emp.JobTitle} , Salary: {emp.Salary}");
            }
        }
        private static void SmallDemartments()
        {
            IQueryable<Departments> departments = from department in _context.Departments
                                                  where department.Employees.Count < 5
                                                  select department;

            foreach (Departments dep in departments)
            {
                Console.WriteLine($"{dep.Name}");
            }
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

            var browns = (
                 from employee in _context.Employees
                 where employee.LastName == "Brown"
                 select employee
             ).ToList();

            browns.ForEach(em => em.Address = address);

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

            var department = (
                from dep in _context.Departments
                where dep.Name == d
                select dep
            ).First();

            foreach (var employee in department.Employees)
            {
                employee.Salary *= (100 + p / 100);
            }
            _context.SaveChanges();
        }
        //????
        private static void DeleteTown()
        { 
            string deletetown = Console.ReadLine();
            var town = (
                from t in _context.Towns
                where t.Name == deletetown
                select t
            ).First();

            _context.Entry(town).Collection(t => t.Addresses).Load();
            _context.Towns.Remove(town);
            _context.SaveChanges();
        }

        private static void ProjectsAudit()
        {
            
        }
    }
}
