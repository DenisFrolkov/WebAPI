using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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
        public DepartmentController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
         }
        [HttpGet]
        public async Task<IActionResult> GetDepartmentForEmployee(Guid employeeId)
        {
            var employee = await _repository.Employee.GetEmployeesAsync(employeeId, trackChanges: false);
            if (employee == null)
            {
                _logger.LogInfo($"Employee with id: {employeeId} doesn't exist in the database.");
                return NotFound();
            }
            var departmentsFromDb = await _repository.Department.GetDepartmentsAsync(employeeId, trackChanges: false);
            var departmentDto = _mapper.Map<IEnumerable<DepartmentDto>>(departmentsFromDb);
            return Ok(departmentDto);
        }

        [HttpGet("{id}", Name = "GetDepartmentForEmployee")]
        public async Task<IActionResult> GetDepartmentForEmployee(Guid employeeId, Guid id)
        {
            var employee = await _repository.Employee.GetEmployeesAsync(employeeId, trackChanges: false);
            if (employee == null)
            {
                _logger.LogInfo($"Company with id: {employeeId} doesn't exist in the database.");
            return NotFound();
            }
            var departmentDb = await _repository.Department.GetDepartmentAsync(employeeId, id,
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
        public async Task<IActionResult> CreateDepartmentForEmployee(Guid employeeId, [FromBody] DepartmentForCreationDto department)
        {
            if (department == null)
            {
                _logger.LogError("DepartmentForCreationDto object sent from client is null.");
                return BadRequest("DepartmentForCreationDto object is null");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the CreateDepartmentForEmployee object");
                return UnprocessableEntity(ModelState);
            }
            var employee = await _repository.Employee.GetEmployeesAsync(employeeId, trackChanges: false);
            if (employee == null)
            {
                _logger.LogError($"Employee with id: {employeeId} doesn't exist in the database.");
                return NotFound($"Employee with id: {employeeId} doesn't exist in the database.");
            }
            var departmentEntity = _mapper.Map<Department>(department);
            _repository.Department.CreateDepartmentForEmployee(employeeId, departmentEntity);
            await _repository.SaveAsync();
            var departmentToReturn = _mapper.Map<DepartmentDto>(departmentEntity);
            return CreatedAtRoute("GetDepartmentForEmployee", new
            {
                employeeId,
                id = departmentToReturn.Id
            }, departmentToReturn);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartmentForEmployee(Guid employeeId, Guid id)
        {
            var employee = await _repository.Employee.GetEmployeesAsync(employeeId, trackChanges: false);
            if (employee == null)
            {
                _logger.LogInfo($"Employee with id: {employeeId} doesn't exist in the database.");
                return NotFound();
            }
            var departmentForEmployee = await _repository.Department.GetDepartmentAsync(employeeId, id, trackChanges: false);
            if (departmentForEmployee == null)
            {
                _logger.LogInfo($"Department with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            _repository.Department.DeleteDepartment(departmentForEmployee);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartmentForEmployee(Guid employeeId, Guid id, [FromBody] DepartmentForUpdateDto department)
        {
            if (department == null)
            {
                _logger.LogError("DepartmentForUpdateDto object sent from client is null.");
                return BadRequest("DepartmentForUpdateDto object is null");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the DepartmentForUpdateDto object");
                return UnprocessableEntity(ModelState);
            }
            var employee = await _repository.Employee.GetEmployeesAsync(employeeId, trackChanges: false);
            if (employee == null)
            {
                _logger.LogInfo($"Employee with id: {employeeId} doesn't exist in the database.");
                return NotFound();
            }
            var departmentEntity = await _repository.Department.GetDepartmentAsync(employeeId, id,
           trackChanges:
            true);
            if (departmentEntity == null)
            {
                _logger.LogInfo($"Department with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            _mapper.Map(department, departmentEntity);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdateDepartmentForEmployee(Guid employeeId, Guid id, [FromBody] JsonPatchDocument<DepartmentForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }
            var employee = await _repository.Employee.GetEmployeesAsync(employeeId, trackChanges: false);
            if (employee == null)
            {
                _logger.LogInfo($"Employee with id: {employeeId} doesn't exist in the database.");
                return NotFound();
            }
            var departmentEntity = await _repository.Department.GetDepartmentAsync(employeeId, id,
           trackChanges:
            true);
            if (departmentEntity == null)
            {
                _logger.LogInfo($"Project with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            var departmentToPatch = _mapper.Map<DepartmentForUpdateDto>(departmentEntity);
            patchDoc.ApplyTo(departmentToPatch, ModelState);
            TryValidateModel(departmentToPatch);
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the patch document");
                return UnprocessableEntity(ModelState);
            }
            _mapper.Map(departmentToPatch, departmentEntity);
            await _repository.SaveAsync();
            return NoContent();
        }
    }
}
