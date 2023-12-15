﻿    using AutoMapper;
using Contracts;
using Entities;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
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
        public class CompanyController : ControllerBase
        {
            private readonly IRepositoryManager _repository;
            private readonly ILoggerManager _logger;
            private readonly IMapper _mapper;
            public CompanyController(IRepositoryManager repository, ILoggerManager
           logger, IMapper mapper)
            {
                _repository = repository;
                _logger = logger;
                _mapper = mapper;
            }

            [HttpGet]
            public async Task<IActionResult> GetCompanies([FromQuery] CompanyParameters companyParameters)
            {
                if (!companyParameters.ValidCompanytNameRange)
                    return BadRequest("A company by that name doesn't exist");
                var companiesFromDb = await _repository.Company.GetAllCompaniesAsync(companyParameters, trackChanges: false);
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(companiesFromDb.MetaData));
                var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companiesFromDb);
                return Ok(companiesDto);
            }

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


            [HttpPost]
            [ServiceFilter(typeof(ValidationFilterAttribute))]
            public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto company)
            {
                var companyEntity = _mapper.Map<Company>(company);
                _repository.Company.CreateCompany(companyEntity);
                await _repository.SaveAsync();
                var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);
                return CreatedAtRoute("CompanyById", new { id = companyToReturn.Id }, companyToReturn);
            }


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

            [HttpDelete("{id}")]
            [ServiceFilter(typeof(ValidateCompanyExistsAttribute))]
            public async Task<IActionResult> DeleteCompany(Guid id)
            {
                var company = HttpContext.Items["company"] as Company;
                _repository.Company.DeleteCompany(company);
                await _repository.SaveAsync();
                return NoContent();
            }

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

            [HttpPatch("{id}")]
            public async Task<IActionResult> PartiallyUpdateCompany(Guid id, [FromBody] JsonPatchDocument<CompanyForUpdateDto> patchDoc)
            {
                if (patchDoc == null)
                {
                    _logger.LogError("patchDoc object sent from client is null.");
                    return BadRequest("patchDoc object is null");
                }
                var company = _repository.Company.GetCompanyAsync(id, trackChanges: true);
                if (company == null)
                {
                    _logger.LogInfo($"Company with id: {id} doesn't exist in the database.");
                    return NotFound();
                }
                var companyEntity = _repository.Company.GetCompanyAsync(id, trackChanges: true);
                if (companyEntity == null)
                {
                    _logger.LogInfo($"Company with id: {id} doesn't exist in the database.");
                    return NotFound();
                }
                var companyToPatch = _mapper.Map<CompanyForUpdateDto>(companyEntity);
                patchDoc.ApplyTo(companyToPatch, ModelState);
                TryValidateModel(companyToPatch);
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid model state for the patch document");
                    return UnprocessableEntity(ModelState);
                }
                _mapper.Map(companyToPatch, companyEntity);
                await _repository.SaveAsync();
                return NoContent();
            }
        }
    }
}
