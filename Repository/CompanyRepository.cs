using Contracts;
using Entities.Models;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        public CompanyRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public void AnyMethodFromCompanyRepository()
        {
            throw new NotImplementedException();
        }
        public void Create1(Entities.Models.Company company) => Create(company);
        public void Create2(Entities.Models.Company anotherCompany) => Create(anotherCompany);
        public void Delete1(Entities.Models.Company olddCompany) => Create(olddCompany);
    }
}

