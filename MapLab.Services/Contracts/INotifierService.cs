using Microsoft.AspNetCore.Http;

namespace MapLab.Services.Contracts
{
    public interface INotifierService
    {
        public Task NotifyAdminsAboutError(Exception exception, HttpContext httpContext);
    }
}
