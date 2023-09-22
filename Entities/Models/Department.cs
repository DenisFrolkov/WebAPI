using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Department
    {
        [Column("DepartmentId")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Department name is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Department manager is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Manager is 30 characters.")]
        public string Manager { get; set; }


        [ForeignKey(nameof(Employee))]
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
