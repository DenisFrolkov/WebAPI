using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class DepartmentForCreationDto
    {
        [Required(ErrorMessage = "Department name is a required field.")]
        [MaxLength(14, ErrorMessage = "Maximum length for the Name is 14 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Department manager is a required field.")]
        [MaxLength(20, ErrorMessage = "Maximum length for the Manager is 20 characters.")]
        public string Manager { get; set; }
    }
}
