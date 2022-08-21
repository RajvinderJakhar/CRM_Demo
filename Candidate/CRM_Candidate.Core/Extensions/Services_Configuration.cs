using CRM_Candidate.Core.RepositoryService;
using CRM_Candidate.Core.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Candidate.Core.Extensions
{
    public static class Services_Configuration
    {
        public static IServiceCollection AddCandidateServices(this IServiceCollection services)
        {
            services.AddScoped<ICandidatesRepositoryService, CandidatesRepositoryService>();
            services.AddScoped<ICandidate_EmployerRepositoryService, Candidate_EmployerRepositoryService>();
            services.AddScoped<IEmailsRepositoryService, EmailsRepositoryService>();
            services.AddScoped<IText_LogsRepositoryService, Text_LogsRepositoryService>();

            services.AddScoped<ICandidatesService, CandidatesService>();
            services.AddScoped<IEmailsService, EmailsService>();
            services.AddScoped<ITextService, TextService>();

            return services;
        }
    }
}
