using Entities.Models;
using Repository.Extensions.Utility.Extensions;
using System.Linq.Dynamic.Core;

namespace Repository.Extensions
{
    public static class RepositoryProjectExtensions
    {
        public static IQueryable<Project> Search(this IQueryable<Project> projects,
        string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return projects;
            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return projects.Where(e => e.Name.ToLower().Contains(lowerCaseTerm));
        }
        public static IQueryable<Project> Sort(this IQueryable<Project> projects, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return projects.OrderBy(e => e.Name);

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Project>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return projects.OrderBy(e => e.Name);
            return projects.OrderBy(orderQuery);
        }
    }
}
