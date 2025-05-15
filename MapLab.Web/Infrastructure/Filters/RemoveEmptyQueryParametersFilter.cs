using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MapLab.Web.Infrastructure.Filters
{
    public class RemoveEmptyQueryParametersFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;
            var queryParams = request.Query.ToDictionary(k => k.Key, v => v.Value.ToString());

            var cleanedQueryParams = queryParams
                .Where(p => !string.IsNullOrEmpty(p.Value) && !IsDefaultValue(context, p.Key, p.Value))
                .ToDictionary(k => k.Key, v => v.Value);

            if (cleanedQueryParams.Count != queryParams.Count)
            {
                var path = request.Path;
                var queryString = string.Join("&", cleanedQueryParams.Select(p => $"{p.Key}={p.Value}"));
                var newUrl = $"{path}{(string.IsNullOrEmpty(queryString) ? "" : "?" + queryString)}";

                context.Result = new RedirectResult(newUrl);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }

        private bool IsDefaultValue(ActionExecutingContext context, string paramName, string paramValue)
        {
            var parameter = context.ActionDescriptor.Parameters
                .FirstOrDefault(p => p.Name.Equals(paramName, StringComparison.OrdinalIgnoreCase));

            if (parameter != null)
            {
                var parameterType = parameter.ParameterType;
                object defaultValue = GetDefaultValue(parameterType);

                if (parameterType == typeof(string) && string.IsNullOrEmpty(paramValue))
                {
                    return true; // String default value is empty
                }

                if (parameterType.IsValueType && paramValue == defaultValue?.ToString())
                {
                    return true; // Value type matches its default value
                }

                if (parameterType == typeof(string) && string.IsNullOrEmpty(paramValue))
                {
                    return true;
                }
            }

            return false;
        }

        // Helper method to get the default value for a type
        private object GetDefaultValue(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
}
