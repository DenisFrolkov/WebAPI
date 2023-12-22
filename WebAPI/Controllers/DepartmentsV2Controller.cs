using Contracts;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/employees/{employeeId}/departments")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v2")]
    public class DepartmentsV2Controller : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        public DepartmentsV2Controller(IRepositoryManager repository, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetDepartmentsForEmployees(Guid employeeId, [FromQuery] DepartmentParameters departmentParameters)
        {
            var employee = await _repository.Employee.GetEmployeesAsync(employeeId, trackChanges: false);
            if (employee == null)
            {
                _logger.LogInfo($"Department with id: {employeeId} doesn't exist in the database.");
                return NotFound();
            }
            var department = await _repository.Department.GetDepartmentsAsync(employeeId, departmentParameters, trackChanges: false);
            return Ok(department);
        }
    }
}
