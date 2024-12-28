using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementWithJWT.Models
{
    public class Department
    {
        [Key]
        public Guid DepartmentId { get; set; }

        [Required]
        [StringLength(100)]
        public string DepartmentName { get; set; }
    }
}
