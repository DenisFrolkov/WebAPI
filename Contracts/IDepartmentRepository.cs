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
        Task<IEnumerable<Department>> GetDepartmentsAsync(Guid employeeId, bool trackChanges);
        Task<Department> GetDepartmentAsync(Guid employeeId, Guid id, bool trackChanges);
        void CreateDepartmentForEmployee(Guid employeeId, Department department);
        void DeleteDepartment(Department department);
    }
}
