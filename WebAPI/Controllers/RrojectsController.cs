using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    namespace CompanyEmployees.Controllers
    {
        [Route("api/projects")]
        [ApiController]
        public class ProjectsController : ControllerBase
        {
            private readonly IRepositoryManager _repository;
            private readonly ILoggerManager _logger;
            private readonly IMapper _mapper;
            public ProjectsController(IRepositoryManager repository, ILoggerManager
           logger, IMapper mapper)
            {
                _repository = repository;
                _logger = logger;
                _mapper = mapper;
            }
            [HttpGet]
            public IActionResult GetProject()
            {
                var projects = _repository.Project.GetAllProjects(trackChanges: false);
                var projectsDto = _mapper.Map<IEnumerable<ProjectDto>>(projects);
                return Ok(projectsDto);
            }
        }
    }
}

