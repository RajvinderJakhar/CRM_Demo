using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Core.Extensions
{
    public static class AuthExtension
    {
        public static T GetService<T>(this IServiceScope scope)
        {
            return scope.ServiceProvider.GetService<T>();
        }
    }
}
