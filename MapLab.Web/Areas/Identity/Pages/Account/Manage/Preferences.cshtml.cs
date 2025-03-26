using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MapLab.Web.Areas.Identity.Pages.Account.Manage
{
    public class PreferencesModel : PageModel
    {
        public List<(string, string)>? AvailableLanguages { get; set; }

        [BindProperty]
        public string? SelectedLanguage { get; set; }

        private readonly RequestLocalizationOptions _localizationOptions;

        public PreferencesModel(IOptions<RequestLocalizationOptions> localizationOptions)
        {
            _localizationOptions = localizationOptions.Value;
        }

        public void OnGet()
        {
            LoadAvailableLanguages();
            SelectedLanguage = CultureInfo.CurrentCulture.Name;
        }

        public IActionResult OnPost()
        {
            // Change the current culture (this is temporary, for persistence store in session/cookie)
            CultureInfo.CurrentCulture = new CultureInfo(SelectedLanguage);
            CultureInfo.CurrentUICulture = new CultureInfo(SelectedLanguage);

            Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(CultureInfo.CurrentCulture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
        );

            // Reload available languages to prevent it from being null
            LoadAvailableLanguages();

            return RedirectToPage();
        }

        private void LoadAvailableLanguages()
        {
            var supportedCultures = _localizationOptions.SupportedCultures;
            AvailableLanguages = supportedCultures?.Select(c => (c.Name, c.NativeName)).ToList();
        }
    }
}
