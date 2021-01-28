using ASPex1.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPex1.Data
{
    public class DummyData
    {
        //Fichero para crear datos de prueba y modificarlos

        public static void Initialize(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<EmployeeContext>();
                context.Database.EnsureDeleted(); //Borro la DB pq haciendo pruebas cada vez se añaden más entradas;
                                                  //En un una futura DB de verdad, eliminar esta línea
                context.Database.EnsureCreated(); //Miramos si la db ha sido creada
                //context.Database.Migrate();
                

                //// Mirar si hay empleados creados con datos
                //if (context.Employees != null && context.Employees.Any())
                //    return;   // DB has already been seeded

                //En el caso que no haya datos, llamamos al método de abajo que nos crea 3 empleados de prueba
                var employees = GetEmployees().ToArray();
                context.Employees.AddRange(employees);
                context.SaveChanges();
            }
        }

        public static List<Employee> GetEmployees()
        {
            List<Employee> employees = new List<Employee>() {
                new Employee {
                Name = "Jim",
                Surname = "Jones",
                Position = "CEO",
                Salary = 3500
                },

                new Employee {
                Name = "Ann",
                Surname = "Petrus",
                Position = "Manager",
                Salary = 2500
                },

                new Employee {
                Name = "Robert",
                Surname = "Tresko",
                Position = "Project Leader",
                Salary = 2000
                }
            };
            return employees;
        }
    }
}
