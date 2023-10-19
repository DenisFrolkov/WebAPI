using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/employees/{employeeId}/departments")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public DepartmentController(IRepositoryManager repository, ILoggerManager
       logger,
        IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
         }
        [HttpGet]
        public IActionResult GetDepartmentForEmployee(Guid employeeId)
        {
            var employee = _repository.Employee.GetEmployees(employeeId, trackChanges: false);
            if (employee == null)
            {
                _logger.LogInfo($"Employee with id: {employeeId} doesn't exist in the database.");
                return NotFound();
            }
            var departmentsFromDb = _repository.Department.GetDepartments(employeeId, trackChanges: false);
            var departmentDto = _mapper.Map<IEnumerable<DepartmentDto>>(departmentsFromDb);
            return Ok(departmentDto);
        }

        [HttpGet("{id}", Name = "GetDepartmentForEmployee")]
        public IActionResult GetDepartmentForEmployee(Guid employeeId, Guid id)
        {
            var employee = _repository.Employee.GetEmployees(employeeId, trackChanges: false);
            if (employee == null)
            {
                _logger.LogInfo($"Company with id: {employeeId} doesn't exist in the database.");
            return NotFound();
            }
            var departmentDb = _repository.Department.GetDepartment(employeeId, id,
           trackChanges:
            false);
            if (departmentDb == null)
            {
                _logger.LogInfo($"Employee with id: {id} doesn't exist in the database.");
            return NotFound();
            }
            var department = _mapper.Map<DepartmentDto>(departmentDb);
            return Ok(department);
        }

        [HttpPost]
        public IActionResult CreateDepartmentForEmployee(Guid employeeId, [FromBody] DepartmentForCreationDto department)
        {
            if (department == null)
            {
                _logger.LogError("DepartmentForCreationDto object sent from client is null.");
                return BadRequest("DepartmentForCreationDto object is null");
            }
            var employee = _repository.Employee.GetEmployees(employeeId, trackChanges: false);
            if (employee == null)
            {
                _logger.LogError($"Employee with id: {employeeId} doesn't exist in the database.");
                return NotFound($"Employee with id: {employeeId} doesn't exist in the database.");
            }
            var departmentEntity = _mapper.Map<Department>(department);
            _repository.Department.CreateDepartmentForEmployee(employeeId, departmentEntity);
            _repository.Save();
            var departmentToReturn = _mapper.Map<DepartmentDto>(departmentEntity);
            return CreatedAtRoute("GetDepartmentForEmployee", new
            {
                employeeId,
                id = departmentToReturn.Id
            }, departmentToReturn);
        }
    }
}
