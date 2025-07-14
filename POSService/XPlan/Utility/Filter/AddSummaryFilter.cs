using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;

namespace XPlan.Utility.Filter
{
    public class AddSummaryFilter : IOperationFilter
    {        
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            string displayName  = context.ApiDescription.ActionDescriptor.DisplayName;
            var keyValuePairs   = GetSummaryInfo();


            foreach (var pair in keyValuePairs)
            {
                if (displayName.Contains(pair.Key))
                {
                    operation.Summary = pair.Value;
                    return;
                }
            }
        }

        protected virtual Dictionary<string, string> GetSummaryInfo()
        {
            // override
            return new Dictionary<string, string>();
        }
    }
}
