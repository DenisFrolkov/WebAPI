using Contracts;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/companies/{companyId}/projects")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v2")]
    public class ProjectsV2Controller : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        public ProjectsV2Controller(IRepositoryManager repository, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetCompanyForProjects(Guid companyId, [FromQuery] ProjectParameters projectParameters)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogInfo($"Project with id: {company} doesn't exist in the database.");
                return NotFound();
            }
            var project = await _repository.Project.GetProjectsAsync(companyId, projectParameters, trackChanges: false);
            return Ok(project);
        }
    }
}
