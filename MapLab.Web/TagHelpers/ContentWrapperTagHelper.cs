using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MapLab.Web.TagHelpers
{
    [HtmlTargetElement("content-wrapper")]
    public class ContentWrapperTagHelper : TagHelper
    {
        public bool Wrap { get; set; }
        public string Class { get; set; } = "viewport-container";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Wrap)
            {
                output.TagName = "div"; // Change tag to <div>
                output.Attributes.SetAttribute("class", Class);
            }
            else
            {
                // Remove the wrapper tag and only keep the inner content
                output.TagName = null;
            }
        }
    }
}
