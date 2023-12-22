using AutoMapper;
using Contracts;
using Entities;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using WebAPI.ActionFilters;
using WebAPI.ModelBinders;
using static System.Collections.Specialized.BitVector32;

namespace WebAPI.Controllers
{
    namespace CompanyEmployees.Controllers
    {
        [Route("api/companies")]
        [ApiController]
        [ApiExplorerSettings(GroupName = "v1")]
        public class CompanyController : ControllerBase
        {
            private readonly IRepositoryManager _repository;
            private readonly ILoggerManager _logger;
            private readonly IMapper _mapper;
            private readonly IDataShaper<CompanyDto> _dataShaper;
            public CompanyController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, IDataShaper<CompanyDto> dataShaper)
            {
                _repository = repository;
                _logger = logger;
                _mapper = mapper;
                _dataShaper = dataShaper;
            }

            /// <summary>
            /// Получает список всех компаний
            /// </summary>
            /// <returns> Список компаний</returns>.
            [HttpGet(Name = "GetCompanies"), Authorize(Roles = "Manager")]
            [HttpHead]
            public async Task<IActionResult> GetCompanies([FromQuery] CompanyParameters companyParameters)
            {
                var companiesFromDb = await _repository.Company.GetAllCompaniesAsync(companyParameters, trackChanges: false);
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(companiesFromDb.MetaData));
                var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companiesFromDb);
                return Ok(_dataShaper.ShapeData(companiesDto, companyParameters.Fields));
            }

            /// <summary>
            /// Получает компанию по Id
            /// </summary>
            /// <param name="id">Id компании</param>
            /// <returns></returns>
            [HttpGet("{id}", Name = "CompanyById")]
            public async Task<IActionResult> GetCompany(Guid id)
            {
                var company = await _repository.Company.GetCompanyAsync(id, trackChanges: false);
                if (company == null)
                {
                    _logger.LogInfo($"Company with id: {id} doesn't exist in the database.");
                    return NotFound();
                }
                else
                {
                    var companyDto = _mapper.Map<CompanyDto>(company);
                    return Ok(companyDto);
                }
            }

            /// <summary>
            /// Получает список определенных компаний по их Id
            /// </summary>
            /// <param name="ids">Id компаний которые хотим получить</param>
            /// <returns></returns>
            [HttpGet("collection/({ids})", Name = "CompanyCollection")]
            public async Task<IActionResult> GetCompanyCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
            {
                if (ids == null)
                {
                    _logger.LogError("Parameter ids is null");
                    return BadRequest("Parameter ids is null");
                }
                var companyEntities = await _repository.Company.GetByIdsAsync(ids, trackChanges: false);
                if (ids.Count() != companyEntities.Count())
                {
                    _logger.LogError("Some ids are not valid in a collection");
                    return NotFound();
                }
                var companiesToReturn =
               _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
                return Ok(companiesToReturn);
            }

            /// <summary>
            /// Создает вновь созданную компанию
            /// </summary>
            /// <param name="company"></param>.
            /// <returns>Вновь созданная компания</returns>.
            /// <response code="201"> Возвращает только что созданный элемент</response>.
            /// <response code="400"> Если элемент равен null</response>.
            /// <код ответа="422"> Если модель недействительна</ответ>.
            [HttpPost(Name = "CreateCompany")]
            [ProducesResponseType(201)]
            [ProducesResponseType(400)]
            [ProducesResponseType(422)]
            [ServiceFilter(typeof(ValidationFilterAttribute))]
            public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto company)
            {
                var companyEntity = _mapper.Map<Company>(company);
                _repository.Company.CreateCompany(companyEntity);
                await _repository.SaveAsync();
                var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);
                return CreatedAtRoute("CompanyById", new { id = companyToReturn.Id }, companyToReturn);
            }

            /// <summary>
            /// Создает список компаний
            /// </summary>
            /// <param name="companyCollection">Коллекция новых компаний</param>
            /// <returns></returns>
            [HttpPost("collection")]
            public async Task<IActionResult> CreateCompanyCollection([FromBody]
            IEnumerable<CompanyForCreationDto> companyCollection)
            {
                if (companyCollection == null)
                {
                    _logger.LogError("Company collection sent from client is null.");
                    return BadRequest("Company collection is null");
                }
                var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);
                foreach (var company in companyEntities)
                {
                    _repository.Company.CreateCompany(company);
                }
                await _repository.SaveAsync();
                var companyCollectionToReturn =
                _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
                var ids = string.Join(",", companyCollectionToReturn.Select(c => c.Id));
                return CreatedAtRoute("CompanyCollection", new { ids },
                companyCollectionToReturn);
            }

            /// <summary>
            /// Удаляет компанию по Id
            /// </summary>
            /// <param name="id">Id компании которую удаляем</param>
            /// <returns></returns>
            [HttpDelete("{id}")]
            [ServiceFilter(typeof(ValidateCompanyExistsAttribute))]
            public async Task<IActionResult> DeleteCompany(Guid id)
            {
                var company = HttpContext.Items["company"] as Company;
                _repository.Company.DeleteCompany(company);
                await _repository.SaveAsync();
                return NoContent();
            }

            /// <summary>
            /// Редактирует компанию по Id
            /// </summary>
            /// <param name="id">d компании которую редактируем</param>
            /// <param name="company">Экземпляр редактированной компании</param>
            /// <returns></returns>
            [HttpPut("{id}")]
            [ServiceFilter(typeof(ValidationFilterAttribute))]
            [ServiceFilter(typeof(ValidateCompanyExistsAttribute))]
            public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyForUpdateDto company)
            {
                var companyEntity = HttpContext.Items["company"] as Company;
                _mapper.Map(company, companyEntity);
                await _repository.SaveAsync();
                return NoContent();
            }

            [HttpOptions]
            public IActionResult GetCompaniesOptions()
            {
                Response.Headers.Add("Allow", "GET, OPTIONS, POST");
                return Ok();
            }
        }
    }
}
