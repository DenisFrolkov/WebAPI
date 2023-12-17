using Contracts;
using Entities.Models;
using Entities;
using Entities.RequestFeatures;
using System.ComponentModel.Design;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;

namespace Repository
{
    public class DepartmentRepository : RepositoryBase<Department>, IDepartmentRepository
    {
        public DepartmentRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public async Task<PagedList<Department>> GetDepartmentsAsync(Guid employeeId, DepartmentParameters departmentParameters, bool trackChanges)
        {
            var departments = await FindByCondition(e => e.EmployeeId.Equals(employeeId), trackChanges)
            .Search(departmentParameters.SearchTerm)
            .OrderBy(e => e.Name)
            .ToListAsync();
            return PagedList<Department>
            .ToPagedList(departments, departmentParameters.PageNumber, departmentParameters.PageSize);
        }

        public async Task<Department> GetDepartmentAsync(Guid employeesId, Guid id, bool trackChanges) =>
        FindByCondition(e => e.EmployeeId.Equals(employeesId) && e.Id.Equals(id), trackChanges).SingleOrDefault();
        public void CreateDepartmentForEmployee(Guid employeeId, Department department)
        {
            department.EmployeeId = employeeId;
            Create(department);
        }
        public void DeleteDepartment(Department department)
        {
            Delete(department);
        }
    }
}
