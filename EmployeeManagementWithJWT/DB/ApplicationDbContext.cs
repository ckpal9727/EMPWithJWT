﻿using EmployeeManagementWithJWT.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementWithJWT.DB
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
    }
}
