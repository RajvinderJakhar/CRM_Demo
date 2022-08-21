using CRM_Core.Service;
using CRM_Core.Utility;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Core.Extensions
{
    public static class Services_Configuration
    {
        public static IServiceCollection AddUtilServices(this IServiceCollection services)
        {
            services.AddScoped<IRemoteRequestService, RemoteRequestService>();

            services.AddScoped<IEmailProvider, EmailProvider>();
            services.AddScoped<ITextProvider, TextProvider>();
            services.AddScoped<IEncryptionUtill, EncryptionUtill>();
            services.AddScoped<IRegexUtill, RegexUtill>();

            return services;
        }
    }
}
