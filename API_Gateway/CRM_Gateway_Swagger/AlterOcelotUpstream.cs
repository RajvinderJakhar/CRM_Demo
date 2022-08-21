using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CRM_Gateway_Swagger
{
    public class AlterOcelotUpstream
    {
        public static string AlterOcelotUpstreamSwaggerJson(HttpContext context, string swaggerJson)
        {
            var swagger = JObject.Parse(swaggerJson);
            // ... alter upstream json
            return swagger.ToString(Formatting.Indented);
        }
    }
}
