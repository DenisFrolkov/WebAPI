using Entities.Models;
using Repository.Extensions.Utility.Extensions;
using System.Linq.Dynamic.Core;

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
        public static IQueryable<Department> Sort(this IQueryable<Department> departments, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return departments.OrderBy(e => e.Name);

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Department>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return departments.OrderBy(e => e.Name);
            return departments.OrderBy(orderQuery);
        }
    }
}
