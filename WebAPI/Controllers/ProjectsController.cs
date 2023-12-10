using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetProjectsForCompany(Guid companyId)
        {
            var company = await _repository.Company.GetCompany(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                return NotFound();
            }
            var projectsFromDb = await _repository.Project.GetProjects(companyId, trackChanges: false);
            var projectsDto = _mapper.Map<IEnumerable<ProjectDto>>(projectsFromDb);
            return Ok(projectsDto);
        }

        [HttpGet("{id}", Name = "GetProjectForCompany")]
        public async Task<IActionResult> GetProjectForCompany(Guid companyId, Guid id)
        {
            var company = await _repository.Company.GetCompany(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                return NotFound();
            }
            var projectDb = await _repository.Project.GetProject(companyId, id, trackChanges: false);
            if (projectDb == null)
            {
                _logger.LogInfo($"Project with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            var project = _mapper.Map<ProjectDto>(projectDb);
            return Ok(project);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProjectForCompany(Guid companyId, [FromBody] ProjectForCreationDto project)
        {
            if (project == null)
            {
                _logger.LogError("ProjectForCreationDto object sent from client is null.");
                return BadRequest("ProjectForCreationDto object is null");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the CreateProjectForCompany object");
                return UnprocessableEntity(ModelState);
            }
            var company = await _repository.Company.GetCompany(companyId, trackChanges: false);
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
        public async Task<IActionResult> DeleteProjectForCompany(Guid companyId, Guid id)
        {
            var company = await _repository.Company.GetCompany(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
            return NotFound();
            }
            var projectForCompany = await _repository.Project.GetProject(companyId, id,
            trackChanges: false);
            if (projectForCompany == null)
            {
                _logger.LogInfo($"Project with id: {id} doesn't exist in the database.");
            return NotFound();
            }
            _repository.Project.DeleteProject(projectForCompany);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProjectForCompany(Guid companyId, Guid id, [FromBody] ProjectForUpdateDto project)
        {
            if (project == null)
            {
                _logger.LogError("ProjectForUpdateDto object sent from client is null.");
            return BadRequest("ProjectForUpdateDto object is null");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the ProjectForUpdateDto object");
                return UnprocessableEntity(ModelState);
            }
            var company = await _repository.Company.GetCompany(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
            return NotFound();
            }
            var projectEntity = await _repository.Project.GetProject(companyId, id,
           trackChanges:
            true);
            if (projectEntity == null)
            {
                _logger.LogInfo($"Project with id: {id} doesn't exist in the database.");
            return NotFound();
            }
            _mapper.Map(project, projectEntity);
            await _repository.SaveAsync();
            return NoContent();
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdateProjectForCompany(Guid companyId, Guid id, [FromBody] JsonPatchDocument<ProjectForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }
            var company = await _repository.Company.GetCompany(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
            return NotFound();
            }
            var projectEntity = await _repository.Project.GetProject(companyId, id,
           trackChanges:
            true);
            if (projectEntity == null)
            {
                _logger.LogInfo($"Project with id: {id} doesn't exist in the database.");
            return NotFound();
            }
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


