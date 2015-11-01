// ==========================================================================
// StyleBundleTagHelper.cs
// Green Parrot Framework
// ==========================================================================
// Copyright (c) Sebastian Stehle
// All rights reserved.
// ========================================================================== 

using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Razor.Runtime.TagHelpers;
using Microsoft.Dnx.Runtime;

namespace SE.Upload.Web.ViewUtils
{
    [HtmlTargetElement("style-bundle", Attributes = AttributeFile)]
    public class StyleBundleTagHelper : BundleTagHelper
    {
        protected override bool AppendVersion => true;

        protected override string Extension => ".css";

        protected override string MinExtension => ".min.css";

        public StyleBundleTagHelper(IApplicationEnvironment applicationEnvironment, IHostingEnvironment hostingEnvironment)
            : base(applicationEnvironment, hostingEnvironment)
        {
        }

        protected override string BuildTag(string file)
        {
            return $"<link type=\"text/css\" rel=\"stylesheet\" href=\"{file}\" />";
        }
    }
}
