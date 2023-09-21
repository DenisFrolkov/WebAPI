using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        public WeatherForecastController(IRepositoryManager repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            var company = new Company { Id = Guid.NewGuid(), Name = "IT_Den", Address = "43 Saransk, B 4", Country = "Russia" };
            var anotherCompany = new Company { Id = Guid.NewGuid(), Name = "IT_Frolkov", Address = "1 Dybenki, I 87", Country = "Russia" };
            var employee = new Employee { Id = new Guid("80abbca8-664d-4b20-b5de-024705497d4a"), Name = "Elena Smit", Age = 21, Position = "Software developer", CompanyId = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870") };
            var anotherEmployee = new Employee { Id = new Guid("021ca3c1-0deb-4afd-ae94-2159a8479811"), Name = "Kane Miller", Age = 28, Position = "Manager", CompanyId = new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3") };
            var oldCompany = new Company { Id = new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3") };
            _repository.Company.Create(company);
            _repository.Company.Create(anotherCompany);
            _repository.Employee.Update(employee);
            _repository.Employee.Update(anotherEmployee);
            _repository.Company.Delete(oldCompany);
            _repository.Save();
            return new string[] { "value1", "value2" };
        }
    }
}