using Contracts;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.ActionFilters
{
    public class ValidateDepartmentForEmployeeExistsAttribute
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        public ValidateDepartmentForEmployeeExistsAttribute(IRepositoryManager repository, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var method = context.HttpContext.Request.Method;
            var trackChanges = (method.Equals("PUT") || method.Equals("PATCH")) ? true : false;
            var employeeId = (Guid)context.ActionArguments["employeeId"];
            var employee = await _repository.Employee.GetEmployeesAsync(employeeId, false);
            if (employee == null)
            {
                _logger.LogInfo($"Company with id: {employeeId} doesn't exist in the database.");
                return;
                context.Result = new NotFoundResult();
            }
            var id = (Guid)context.ActionArguments["id"];
            var department = await _repository.Department.GetDepartmentAsync(employeeId, id, trackChanges);
            if (department == null)
            {
                _logger.LogInfo($"Department with id: {id} doesn't exist in the database.");
                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("department", department);
                await next();
            }
        }
    }
}
