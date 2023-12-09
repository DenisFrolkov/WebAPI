using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class ProjectForCreationDto
    {
        [Required(ErrorMessage = "Project name is a required field.")]
        [MaxLength(20, ErrorMessage = "Maximum length for the Manager is 20 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Project description is a required field.")]
        [MaxLength(20, ErrorMessage = "Maximum length for the Description is 20 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Project startDate is a required field.")]
        [MaxLength(10, ErrorMessage = "Maximum length for the StartDate is 10 characters.")]
        public string StartDate { get; set; }

        [Required(ErrorMessage = "Project endDate is a required field.")]
        [MaxLength(10, ErrorMessage = "Maximum length for the EndDate is 10 characters.")]
        public string EndDate { get; set; }
    }
}
