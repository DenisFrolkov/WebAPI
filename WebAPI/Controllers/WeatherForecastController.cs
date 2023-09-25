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
            _repository.Save();
            return new string[] { "value1", "value2" };
        }
    }
}