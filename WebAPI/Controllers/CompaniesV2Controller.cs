using Contracts;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompaniesV2Controller : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        public CompaniesV2Controller(IRepositoryManager repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public async Task<IActionResult> GetCompanies([FromQuery] CompanyParameters companyParameters)
        {
            var companies = await _repository.Company.GetAllCompaniesAsync(companyParameters, trackChanges: false);
            return Ok(companies);
        }
    }
}
