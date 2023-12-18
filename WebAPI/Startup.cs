using Microsoft.AspNetCore.HttpOverrides;
using NLog;
using AutoMapper;
using WebAPI.Extensions;
using Contracts;
using Microsoft.AspNetCore.Mvc;
using WebAPI.ActionFilters;
using Entities.DataTransferObjects;
using Repository.DataShaping;

namespace WebAPI;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(),
       "/nlog.config"));
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.ConfigureCors();
        services.ConfigureIISIntegration();
        services.ConfigureLoggerService();
        services.ConfigureSqlContext(Configuration);
        services.ConfigureRepositoryManager();
        services.AddControllers();
        services.AddAutoMapper(typeof(Startup));
        services.AddControllers(config =>
        {
            config.RespectBrowserAcceptHeader = true;
            config.ReturnHttpNotAcceptable = true;
        }).AddNewtonsoftJson()
            .AddXmlDataContractSerializerFormatters()
            .AddCustomCSVFormatter();
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
        services.AddScoped<ValidationFilterAttribute>();
        services.AddScoped<ValidateCompanyExistsAttribute>();
        services.AddScoped<ValidateEmployeeForCompanyExistsAttribute>();
        services.AddScoped<ValidateDepartmentForEmployeeExistsAttribute>();
        services.AddScoped<ValidateProjectForCompanyExistsAttribute>();
        services.AddScoped<IDataShaper<CompanyDto>, DataShaper<CompanyDto>>();
        services.AddScoped<IDataShaper<EmployeeDto>, DataShaper<EmployeeDto>>();
        services.AddScoped<IDataShaper<DepartmentDto>, DataShaper<DepartmentDto>>();
        services.AddScoped<IDataShaper<ProjectDto>, DataShaper<ProjectDto>>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
    ILoggerManager logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.ConfigureExceptionHandler(logger);
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("CorsPolicy");
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
}