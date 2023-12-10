using Contracts;
using Entities.Models;
using Entities;

namespace Repository
{
    public class DepartmentRepository : RepositoryBase<Department>, IDepartmentRepository
    {
        public DepartmentRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Department>> GetDepartmentsAsync(Guid employeeId, bool trackChanges) =>
        FindByCondition(e => e.EmployeeId.Equals(employeeId), trackChanges)
        .OrderBy(e => e.Name);

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
