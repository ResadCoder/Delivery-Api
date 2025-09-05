using Microsoft.AspNetCore.Mvc.Filters;

namespace DeliveryAPi.Api.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class ValidateIdAttribute(params string[] paramsArr) : ActionFilterAttribute
{
    private readonly string[] _paramsArr = paramsArr.Length == 0 ? ["id"] : paramsArr;

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        foreach (var paramName in _paramsArr)
        {
            bool isValid = context.RouteData.Values.TryGetValue(paramName, out var routeValue)
                           && int.TryParse(routeValue?.ToString(), out int routeId)
                           && routeId > 0;

            if (!isValid && context.HttpContext.Request.Query.TryGetValue(paramName, out var queryValue)
                         && int.TryParse(queryValue.ToString(), out int queryId)
                         && queryId > 0)
            {
                isValid = true;
            }
            
            if (!isValid)
            {
                throw new Exception("Invalid parameter");
            }
        }
    }

}