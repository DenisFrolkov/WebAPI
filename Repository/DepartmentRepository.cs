﻿using Contracts;
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

        public IEnumerable<Department> GetDepartments(Guid employeeId, bool trackChanges) =>
        FindByCondition(e => e.EmployeeId.Equals(employeeId), trackChanges)
        .OrderBy(e => e.Name);

        public Department GetDepartment(Guid employeesId, Guid id, bool trackChanges) =>
        FindByCondition(e => e.EmployeeId.Equals(employeesId) && e.Id.Equals(id), trackChanges).SingleOrDefault();
    }
}
