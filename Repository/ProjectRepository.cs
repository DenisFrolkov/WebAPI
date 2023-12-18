using Contracts;
using Entities.Models;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;

namespace Repository
{
    public class ProjectRepository : RepositoryBase<Project>, IProjectRepository
    {
        public ProjectRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }


        public async Task<PagedList<Project>> GetProjectsAsync(Guid companyId, ProjectParameters projectParameters, bool trackChanges)
        {
            var projects = await FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges)
            .Search(projectParameters.SearchTerm)
            .Sort(projectParameters.OrderBy)
            .ToListAsync();
            return PagedList<Project>
            .ToPagedList(projects, projectParameters.PageNumber, projectParameters.PageSize);
        }

        public async Task<Project> GetProjectAsync(Guid companyId, Guid id, bool trackChanges) =>
        FindByCondition(e => e.CompanyId.Equals(companyId) && e.Id.Equals(id), trackChanges).SingleOrDefault();

        public void CreateProjectForCompany(Guid companyId, Project project)
        {
            project.CompanyId = companyId;
            Create(project);
        }
        public void DeleteProject(Project project)
        {
            Delete(project);
        }
    }
}
