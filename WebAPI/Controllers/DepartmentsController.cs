using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repository.DataShaping;
using WebAPI.ActionFilters;

namespace WebAPI.Controllers
{
    [Route("api/employees/{employeeId}/departments")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IDataShaper<DepartmentDto> _dataShaper;
        public DepartmentController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, IDataShaper<DepartmentDto> dataShaper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _dataShaper = dataShaper;
        }
        [HttpGet]
        public async Task<IActionResult> GetDepartmentsForEmployee(Guid employeeId, [FromQuery] DepartmentParameters departmentParameters)
        {
            var employee = await _repository.Employee.GetEmployeesAsync(employeeId, trackChanges: false);
            if (employee == null)
            {
                _logger.LogInfo($"Employee with id: {employeeId} doesn't exist in the database.");
                return NotFound();
            }
            var departmentsFromDb = await _repository.Department.GetDepartmentsAsync(employeeId, departmentParameters, trackChanges: false);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(departmentsFromDb.MetaData));
            var departmentDto = _mapper.Map<IEnumerable<DepartmentDto>>(departmentsFromDb);
            return Ok(_dataShaper.ShapeData(departmentDto, departmentParameters.Fields));
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
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateDepartmentForEmployee(Guid employeeId, [FromBody] DepartmentForCreationDto department)
        {
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
        [ServiceFilter(typeof(ValidateDepartmentForEmployeeExistsAttribute))]
        public async Task<IActionResult> DeleteDepartmentForEmployee(Guid employeeId, Guid id)
        {
            var departmentForEmployee = HttpContext.Items["department"] as Department;
            _repository.Department.DeleteDepartment(departmentForEmployee);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateDepartmentForEmployeeExistsAttribute))]
        public async Task<IActionResult> UpdateDepartmentForEmployee(Guid employeeId, Guid id, [FromBody] DepartmentForUpdateDto department)
        {
            var departmentEntity = HttpContext.Items["department"] as Department;
            _mapper.Map(department, departmentEntity);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        [ServiceFilter(typeof(ValidateDepartmentForEmployeeExistsAttribute))]
        public async Task<IActionResult> PartiallyUpdateDepartmentForEmployee(Guid employeeId, Guid id, [FromBody] JsonPatchDocument<DepartmentForUpdateDto> patchDoc)
        {
            var departmentEntity = HttpContext.Items["department"] as Department;
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
