using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ASPNETBlank.TagHelpers
{
    public class HashToLinkTagHelper : TagHelper
    {
        public string Hash { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext Context { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            output.Content.Append(Context.HttpContext.Request.Scheme + "://" + Context.HttpContext.Request.Host.ToString() + "/" + Hash);
            output.Attributes.Add("href", Hash);
        }
    }
}
