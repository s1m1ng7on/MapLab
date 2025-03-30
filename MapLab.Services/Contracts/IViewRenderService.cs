namespace MapLab.Services.Contracts
{
    public interface IViewRenderService
    {
        public Task<string> RenderViewToStringAsync(string viewName, object model);
    }
}
