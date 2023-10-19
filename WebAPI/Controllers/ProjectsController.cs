﻿using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Http;
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
        public IActionResult GetProjectsForCompany(Guid companyId)
        {
            var company = _repository.Company.GetCompany(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                return NotFound();
            }
            var projectsFromDb = _repository.Project.GetProjects(companyId, trackChanges: false);
            var projectsDto = _mapper.Map<IEnumerable<ProjectDto>>(projectsFromDb);
            return Ok(projectsDto);
        }

        [HttpGet("{id}", Name = "GetProjectForCompany")]
        public IActionResult GetProjectForCompany(Guid companyId, Guid id)
        {
            var company = _repository.Company.GetCompany(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                return NotFound();
            }
            var projectDb = _repository.Project.GetProject(companyId, id, trackChanges: false);
            if (projectDb == null)
            {
                _logger.LogInfo($"Project with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            var project = _mapper.Map<ProjectDto>(projectDb);
            return Ok(project);
        }

        [HttpPost]
        public IActionResult CreateProjectForCompany(Guid companyId, [FromBody] ProjectForCreationDto project)
        {
            if (project == null)
            {
                _logger.LogError("ProjectForCreationDto object sent from client is null.");
                return BadRequest("ProjectForCreationDto object is null");
            }
            var company = _repository.Company.GetCompany(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogError($"Company with id: {companyId} doesn't exist in the database."); // Исправлен вызов LogInfo на LogError
                return NotFound($"Company with id: {companyId} doesn't exist in the database.");
            }
            var projectEntity = _mapper.Map<Project>(project);
            _repository.Project.CreateProjectForCompany(companyId, projectEntity);
            _repository.Save();
            var projectToReturn = _mapper.Map<ProjectDto>(projectEntity);
            return CreatedAtRoute("GetProjectForCompany", new // Исправлено имя маршрута на "GetProjectForCompany"
            {
                companyId,
                id = projectToReturn.Id
            }, projectToReturn);
        }

    }
}


