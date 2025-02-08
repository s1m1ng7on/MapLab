using Ganss.Xss;

namespace MapLab.Web.Models.Shared
{
    public class ModalViewModel
    {
        private string? _body;

        private static readonly HtmlSanitizer _sanitizer = new HtmlSanitizer();

        public string Id { get; set; } = "modal-" + Guid.NewGuid().ToString("N");

        public string? Title { get; set; }

        public string? Body
        {
            get => _body;
            set => _body = _sanitizer.Sanitize(value);
        }

        public string LeftButtonText { get; set; } = "Back";

        public string RightButtonText { get; set; } = "Next";
    }
}
