using EmployeeManagementWithJWT.DB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementWithJWT.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public EmployeeController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        [Authorize]
        public IActionResult Index()
        {
            var employees = _applicationDbContext.Employees.Include(e => e.Department).ToList();  // Include Department for display
            return View(employees);
        }
    }
}
