using System;
using System.Linq;
using EmployeesDB.Data.Models;

namespace EmployeesDB
{
    internal class Program
    {
        private static readonly EmployeesContext _context = new EmployeesContext();

        private static void Main(string[] args)
        {
            Task_5();
        }


        private static void Task_1()
        {
            IQueryable<Employees> employees = from employee in _context.Employees
                where employee.Salary > 48000
                orderby employee.LastName descending
                select employee;

            foreach (var emp in employees)
                Console.WriteLine(
                    $"{emp.LastName} {emp.FirstName} {emp.MiddleName} - {emp.JobTitle} , Salary: {emp.Salary}");
        }

        
        private static void Task_2()
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

        private static void Task_3()
        {
        }

        
        private static void Task_4()
        {
            var id = int.Parse(Console.ReadLine());
            var employee = (
                from e in _context.Employees
                where e.EmployeeId == id
                select e
            ).FirstOrDefault();
            var projects = (
                from ep in _context.EmployeesProjects
                where ep.EmployeeId == id
                select ep.Project
            ).ToList();
            if (employee != null)
            {
                Console.WriteLine(
                    $"{employee.LastName} {employee.FirstName} {employee.MiddleName} - {employee.JobTitle}");
                foreach (var project in projects) Console.WriteLine($"{project.Name}");
            }
        }

        private static void Task_5()
        {
            var departments = from department in _context.Departments
                where department.Employees.Count < 5
                select department;

            foreach (var dep in departments) Console.WriteLine($"{dep.Name}");
        }

        
        private static void Task_6()
        {
            var d = Console.ReadLine();
            var p = int.Parse(Console.ReadLine());

            var department = (
                from dep in _context.Departments
                where dep.Name == d
                select dep
            ).First();

            foreach (var employee in department.Employees) employee.Salary *= 100 + p / 100;
            _context.SaveChanges();
        }

        
        private static void Task_7()
        {
            var id = int.Parse(Console.ReadLine());

            var department = (
                from dep in _context.Departments
                where dep.DepartmentId == id
                select dep
            ).First();

            if (department != null)
            {
                _context.Departments.Remove(department);
                _context.SaveChanges();
            }
        }

        
        private static void Task_8()
        {
            var deleteTown = Console.ReadLine();
            var town = (
                from t in _context.Towns
                where t.Name == deleteTown
                select t
            ).First();

            _context.Entry(town).Collection(t => t.Addresses).Load();
            _context.Towns.Remove(town);
            _context.SaveChanges();
        }
    }
}