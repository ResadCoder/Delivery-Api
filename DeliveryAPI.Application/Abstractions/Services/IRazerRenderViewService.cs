namespace DeliveryAPI.Application.Abstractions;

public interface IRazerRenderViewService
{
    Task<string> RenderViewToStringAsync<TModel>(string viewName, TModel model);

}