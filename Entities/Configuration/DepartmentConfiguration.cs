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
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasData
            (
                new Department
                {
                    Id = new Guid("9ce18e7b-82b3-4be1-bd02-bdc34cdbf10e"),
                    Name = "IT Department",
                    Manager = "John Doe",
                    EmployeeId = new Guid("80abbca8-664d-4b20-b5de-024705497d4a")
                },
                new Department
                {
                    Id = new Guid("2a30ca60-117b-4b2d-a52d-5d912d7d2676"),
                    Name = "HR Department",
                    Manager = "Jane Smith",
                    EmployeeId = new Guid("86dba8c0-d178-41e7-938c-ed49778fb52a")
                }
            );
        }
    }
}
