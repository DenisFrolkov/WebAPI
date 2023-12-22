using Contracts;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/companies/{companyId}/employees")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v2")]
    public class EmployeesV2Controller : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        public EmployeesV2Controller(IRepositoryManager repository, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetEmployeesForCompany(Guid companyId, [FromQuery] EmployeeParameters employeeParameters)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                return NotFound();
            }
            var employees = await _repository.Employee.GetEmployeesAsync(companyId, employeeParameters, trackChanges: false);
            return Ok(employees);
        }
    }
}
