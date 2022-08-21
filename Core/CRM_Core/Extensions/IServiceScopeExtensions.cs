using Microsoft.Extensions.DependencyInjection;

namespace CRM_Core.Extensions
{
    public static class IServiceScopeExtensions
    {
        public static T GetService<T>(this IServiceScope scope)
        {
            return scope.ServiceProvider.GetService<T>();
        }
    }
}
