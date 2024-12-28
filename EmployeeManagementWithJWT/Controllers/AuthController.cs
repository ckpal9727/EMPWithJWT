using EmployeeManagementWithJWT.DB;
using EmployeeManagementWithJWT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeeManagementWithJWT.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IConfiguration _configuration;

        public AuthController(ApplicationDbContext applicationDbContext, IConfiguration configuration)
        {
            _applicationDbContext = applicationDbContext;
            _configuration = configuration;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var employee = await _applicationDbContext.Employees.Include(d => d.Department).Where(e => e.Email == email && e.Password == password).FirstOrDefaultAsync();
            if (employee != null)
            {
                var token=GenerateJwtToken(employee);
                HttpContext.Response.Headers.Add("Authorization", $"Bearer {token}");
                TempData["Message"] = "Login Successful! Token generated.";
                return RedirectToAction("index", "Home");
            }
            TempData["Error"] = "Invalid Username or Password.";
            return View();
        }
        private string GenerateJwtToken(Employee employee)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, employee.Email),
                new Claim(ClaimTypes.Role, employee.Department.DepartmentName),
            };
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
              _configuration["Jwt:Issuer"],
               claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public IActionResult Logout()
        {
            HttpContext.Response.Headers.Remove("Authorization");
            TempData["Message"] = "Logged out successfully.";
            return RedirectToAction("Login");
        }
    }
}
