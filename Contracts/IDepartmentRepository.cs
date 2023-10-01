using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IDepartmentRepository
    {
        IEnumerable<Department> GetDepartments(Guid employeeId, bool trackChanges);
        Department GetDepartment(Guid employeeId, Guid id, bool trackChanges);
    }
}
