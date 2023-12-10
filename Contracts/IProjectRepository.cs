using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetProjectsAsync(Guid companyId, bool trackChanges);
        Task<Project> GetProjectAsync(Guid companyId, Guid id, bool trackChanges);
        void CreateProjectForCompany(Guid companyId, Project project);
        void DeleteProject(Project project);

    }
}
