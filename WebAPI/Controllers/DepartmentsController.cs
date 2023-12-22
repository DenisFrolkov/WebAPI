using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
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
    [ApiExplorerSettings(GroupName = "v1")]
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

        /// <summary>
        /// Получает определый отдел для определенного сотрудника
        /// </summary>
        /// <param name="employeeId">Id сотрудника</param>
        /// <param name="id">Id отедла который хотим получить</param>
        /// <returns></returns>
        [HttpGet(Name = "GetDepartmentsForEmployee"), Authorize(Roles = "Manager")]
        [HttpHead]
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

        /// <summary>
        /// Получает определый отдел для определенного сотрудника
        /// </summary>
        /// <param name="employeeId">Id сотрудника</param>
        /// <param name="id">Id отедла который хотим получить</param>
        /// <returns></returns>
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

        /// <summary>
        /// Созает сотрудника для определенной компании
        /// </summary>
        /// <param name="department">Id сотрудника</param>
        /// <param name="department">Экземпляр нового отдела</param>
        /// <returns></returns>
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

        /// <summary>
        /// Удаляет отдел для определенного сотрудника
        /// </summary>
        /// <param name="employeeId">Id сотрудника</param>
        /// <param name="id">Id отедал который хотим удалить</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateDepartmentForEmployeeExistsAttribute))]
        public async Task<IActionResult> DeleteDepartmentForEmployee(Guid employeeId, Guid id)
        {
            var departmentForEmployee = HttpContext.Items["department"] as Department;
            _repository.Department.DeleteDepartment(departmentForEmployee);
            await _repository.SaveAsync();
            return NoContent();
        }

        /// <summary>
        /// Редактирует отедл для определенного сотрудника
        /// </summary>
        /// <param name="employeeId">Id сотрудника</param>
        /// <param name="id">Id отдела который редактируем</param>
        /// <param name="department">Экземпляр редактированного отдела</param>
        /// <returns></returns>
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

        /// <summary>
        /// Редактирует отдел для определенного сотрудника
        /// </summary>
        /// <param name="employeeId">Id сотрудника</param>
        /// <param name="id">Id отдела который редактируем</param>
        /// <param name="patchDoc">Параметры для patch запроса</param>
        /// <returns></returns>
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

        [HttpOptions]
        public IActionResult GetDepartmentsOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST");
            return Ok();
        }
    }
}
