using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Extensions
{
    public static class RepositoryCompanyExtensions
    {
        public static IQueryable<Company> Search(this IQueryable<Company> companies, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return companies;
            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return companies.Where(e => e.Name.ToLower().Contains(lowerCaseTerm));
        }
    }
}
