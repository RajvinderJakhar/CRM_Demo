using CRM_Employer.Core.RepositoryService;
using CRM_Employer.Core.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Employer.Core.Extensions
{
    public static class Services_Configuration
    {
        public static IServiceCollection AddEmployerServices(this IServiceCollection services)
        {
            services.AddScoped<IEmail_TemplatesRepositoryService, Email_TemplatesRepositoryService>();
            services.AddScoped<IEmployersRepositoryService, EmployersRepositoryService>();
            services.AddScoped<IText_TemplatesRepositoryService, Text_TemplatesRepositoryService>();

            services.AddScoped<IEmployerService, EmployerService>();
            services.AddScoped<ITemplateService, TemplateService>();

            return services;
        }
    }
}
