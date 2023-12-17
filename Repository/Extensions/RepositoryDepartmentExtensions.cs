using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Extensions
{
    public static class RepositoryDepartmentExtensions
    {
        public static IQueryable<Department> Search(this IQueryable<Department> departments,
        string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return departments;
            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return departments.Where(e => e.Name.ToLower().Contains(lowerCaseTerm));
        }
    }
}
