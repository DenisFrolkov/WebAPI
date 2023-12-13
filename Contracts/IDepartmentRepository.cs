using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IDepartmentRepository
    {
        Task<PagedList<Department>> GetDepartmentsAsync(Guid employeeId, DepartmentParameters departmentParameters, bool trackChanges);
        Task<Department> GetDepartmentAsync(Guid employeeId, Guid id, bool trackChanges);
        void CreateDepartmentForEmployee(Guid employeeId, Department department);
        void DeleteDepartment(Department department);
    }
}
