using Microsoft.AspNetCore.Mvc.Filters;

namespace DeliveryAPi.Api.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class PaginationValidate: ActionFilterAttribute
{

    private readonly int _maxTake ;
    public PaginationValidate(int maxTake = 10)
    {
        if (maxTake <= 0) throw new ArgumentOutOfRangeException(nameof(maxTake));
        if (maxTake > 100) throw new ArgumentOutOfRangeException(nameof(maxTake));
        _maxTake = maxTake;
    }
    
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ActionArguments.TryGetValue("page", out var pageObj) && pageObj is int page)
        {
            if (page <= 0) throw new Exception("Səhifə 1-dən kiçik ola bilməz!"); 
        }

        if (context.ActionArguments.TryGetValue("take", out var takeObj) && takeObj is int take)
        {
            if (take <= 0) throw new Exception("Say 1-dən kiçik ola bilməz!");
            if (take > _maxTake) throw new Exception($"Say maksimum {_maxTake} ola bilər!");

        }
    }
    
}