﻿using Contracts;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace WebAPI.Controllers
{
    namespace CompanyEmployees.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class CompaniesController : ControllerBase
        {
            private readonly IRepositoryManager _repository;
            private readonly ILoggerManager _logger;
            public CompaniesController(IRepositoryManager repository, ILoggerManager
           logger)
            {
                _repository = repository;
                _logger = logger;
            }
            [HttpGet]
            public IActionResult GetCompanies()
            {
                try
                {
                    var companies = _repository.Company.GetAllCompanies(trackChanges: false);
                    var companiesDto = companies.Select(c => new CompanyDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        FullAddress = string.Join(' ', c.Address, c.Country)
                    }).ToList();
                    return Ok(companiesDto);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Something went wrong in the action { ex}");
                return StatusCode(500, "Internal server error");
                }
            }
        }
    }
}
