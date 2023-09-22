using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Project
    {
        [Column("ProjectId")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Project name is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string Name { get; set; }

        [MaxLength(100, ErrorMessage = "Maximum length for the Description is 100 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Project start date is a required field.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Project end date is a required field.")]
        public DateTime EndDate { get; set; }


        [ForeignKey(nameof(Company))]
        public Guid CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
