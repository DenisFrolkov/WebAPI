﻿using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IProjectRepository
    {
        IEnumerable<Project> GetProjects(Guid companyId, bool trackChanges);
        Project GetProject(Guid companyId, Guid id, bool trackChanges);
        void CreateProjectForCompany(Guid companyId, Project project);

    }
}
