using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebAPI.ActionFilters;

namespace WebAPI.Controllers
{
    [Route("api/companies/{companyId}/projects")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public ProjectsController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetProjectsForCompany(Guid companyId, [FromQuery] ProjectParameters projectParameters)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                return NotFound();
            }
            var projectsFromDb = await _repository.Project.GetProjectsAsync(companyId, projectParameters, trackChanges: false);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(projectsFromDb.MetaData));
            var projectsDto = _mapper.Map<IEnumerable<ProjectDto>>(projectsFromDb);
            return Ok(projectsDto);
        }

        [HttpGet("{id}", Name = "GetProjectForCompany")]
        public async Task<IActionResult> GetProjectForCompany(Guid companyId, Guid id)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                return NotFound();
            }
            var projectDb = await _repository.Project.GetProjectAsync(companyId, id, trackChanges: false);
            if (projectDb == null)
            {
                _logger.LogInfo($"Project with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            var project = _mapper.Map<ProjectDto>(projectDb);
            return Ok(project);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateProjectForCompany(Guid companyId, [FromBody] ProjectForCreationDto project)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogError($"Company with id: {companyId} doesn't exist in the database."); // Исправлен вызов LogInfo на LogError
                return NotFound($"Company with id: {companyId} doesn't exist in the database.");
            }
            var projectEntity = _mapper.Map<Project>(project);
            _repository.Project.CreateProjectForCompany(companyId, projectEntity);
            await _repository.SaveAsync();
            var projectToReturn = _mapper.Map<ProjectDto>(projectEntity);
            return CreatedAtRoute("GetProjectForCompany", new // Исправлено имя маршрута на "GetProjectForCompany"
            {
                companyId,
                id = projectToReturn.Id
            }, projectToReturn);
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateProjectForCompanyExistsAttribute))]
        public async Task<IActionResult> DeleteProjectForCompany(Guid companyId, Guid id)
        {
            var projectForCompany = HttpContext.Items["project"] as Project;
            _repository.Project.DeleteProject(projectForCompany);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateEmployeeForCompanyExistsAttribute))]
        public async Task<IActionResult> UpdateProjectForCompany(Guid companyId, Guid id, [FromBody] ProjectForUpdateDto project)
        {
            var projectEntity = HttpContext.Items["project"] as Project;
            _mapper.Map(project, projectEntity);
            await _repository.SaveAsync();
            return NoContent();
        }
        [HttpPatch("{id}")]
        [ServiceFilter(typeof(ValidateEmployeeForCompanyExistsAttribute))]
        public async Task<IActionResult> PartiallyUpdateProjectForCompany(Guid companyId, Guid id, [FromBody] JsonPatchDocument<ProjectForUpdateDto> patchDoc)
        {
            var projectEntity = HttpContext.Items["project"] as Project;
            var projectToPatch = _mapper.Map<ProjectForUpdateDto>(projectEntity);
            patchDoc.ApplyTo(projectToPatch, ModelState);
            TryValidateModel(projectToPatch);
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the patch document");
                return UnprocessableEntity(ModelState);
            }
            _mapper.Map(projectToPatch, projectEntity);
            await _repository.SaveAsync();
            return NoContent();
        }
    }
}


