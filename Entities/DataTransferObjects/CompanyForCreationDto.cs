using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class CompanyForCreationDto
    {
        [Required(ErrorMessage = "Company name is a required field.")]
        [MaxLength(14, ErrorMessage = "Maximum length for the Name is 14 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Employee address is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the Address is 60 characters.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Employee country is a required field.")]
        [MaxLength(20, ErrorMessage = "Maximum length for the Country is 20 characters.")]
        public string Country { get; set; }
    }
}
