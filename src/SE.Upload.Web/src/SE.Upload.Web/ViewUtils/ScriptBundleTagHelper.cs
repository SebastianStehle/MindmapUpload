// ==========================================================================
// ScriptBundleTagHelper.cs
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
    [HtmlTargetElement("script-bundle", Attributes = AttributeFile)]
    public class ScriptBundleHelper : BundleTagHelper
    {
        protected override bool AppendVersion => true;

        protected override string Extension => ".js";

        protected override string MinExtension => ".min.js";

        public ScriptBundleHelper(IApplicationEnvironment applicationEnvironment, IHostingEnvironment hostingEnvironment)
            : base(applicationEnvironment, hostingEnvironment)
        {
        }

        protected override string BuildTag(string file)
        {
            return $"<script type=\"text/javascript\" src=\"{file}\"></script>";
        }
    }
}
