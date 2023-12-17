using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
