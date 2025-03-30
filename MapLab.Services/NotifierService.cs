using MapLab.Data.Entities;
using MapLab.Data.Managers;
using MapLab.Services.Contracts;
using MapLab.Shared.Models.Emails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Reflection;

namespace MapLab.Services
{
    public class NotifierService : INotifierService
    {
        private readonly IEmailSender _emailSender;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly ProfileManager<Profile> _profileManager;
        private readonly IViewRenderService _viewRenderService;

        public NotifierService(IEmailSender emailSender, IHostEnvironment hostEnvironment, ProfileManager<Profile> profileManager, IViewRenderService viewRenderService)
        {
            _emailSender = emailSender;
            _hostEnvironment = hostEnvironment;
            _profileManager = profileManager;
            _viewRenderService = viewRenderService;
        }

        public async Task NotifyAdminsAboutError(Exception exception, HttpContext httpContext)
        {
            var errorViewModel = new ErrorViewModel
            {
                ErrorMessage = exception.Message,
                TimeOccured = DateTime.UtcNow,
                StatusCode = httpContext.Response.StatusCode,
                RequestId = Activity.Current?.Id ?? httpContext.TraceIdentifier,
                RequestUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}{httpContext.Request.QueryString}",
                RequestMethod = httpContext.Request.Method,
                UserIpAddress = httpContext.Connection.RemoteIpAddress?.ToString(),
                Username = httpContext.User?.Identity?.Name,
                UserAgent = httpContext.Request.Headers.UserAgent,
                ExceptionType = exception.GetType().FullName,
                ExceptionMessage = exception.Message,
                StackTrace = exception.StackTrace,
                InnerException = exception.InnerException?.ToString(),
                Environment = _hostEnvironment.EnvironmentName,
                ApplicationVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString()
            };

            var adminUsers = await _profileManager.GetUsersInRoleAsync("Admin");

            var adminEmailUserNameList = adminUsers
                .Select(p => (p.Email, p.UserName))
                .ToList();

            var subject = "Server Error Notification";
            var htmlMessage = await _viewRenderService.RenderViewToStringAsync("Emails/Error", errorViewModel);

            foreach ((var adminEmail, var adminUserName) in adminEmailUserNameList)
            {
                await _emailSender.SendEmailAsync(adminEmail, subject, htmlMessage);
            }
        }
    }
}
