using Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Configuration
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasData
            (
                new Project
                {
                    Id = new Guid("0c9de349-d07a-4752-8e32-90fdfa01ad1c"),
                    Name = "Project A",
                    Description = "Description for Project A",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(30),
                    CompanyId = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870")
                },
                new Project
                {
                    Id = new Guid("b5ba1486-f2d4-48b5-b78e-4cc807bcaffd"),
                    Name = "Project B",
                    Description = "Description for Project B",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(45),
                    CompanyId = new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3")
                }
            );
        }
    }
}
