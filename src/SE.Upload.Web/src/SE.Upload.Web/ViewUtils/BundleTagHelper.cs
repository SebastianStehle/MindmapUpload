// ==========================================================================
// BundleTagHelper.cs
// Green Parrot Framework
// ==========================================================================
// Copyright (c) Sebastian Stehle
// All rights reserved.
// ========================================================================== 

using System;
using System.IO;
using System.Text;
using Microsoft.AspNet.FileProviders;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.ViewFeatures;
using Microsoft.AspNet.Razor.Runtime.TagHelpers;
using Microsoft.Dnx.Runtime;

namespace SE.Upload.Web.ViewUtils
{
    public abstract class BundleTagHelper : TagHelper
    {
        public const string AttributeBase = "base";
        public const string AttributeFile = "file";
        public const string AttributeBundle = "bundle";
        public const string AttributeMinified = "minified";

        private readonly IApplicationEnvironment applicationEnvironment;
        private readonly IHostingEnvironment hostingEnvironment;

        [HtmlAttributeNotBound, ViewContext]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName(AttributeBase)]
        public string Base { get; set; }

        [HtmlAttributeName(AttributeFile)]
        public string File { get; set; }

        [HtmlAttributeName(AttributeBundle)]
        public bool UseBundle { get; set; }

        [HtmlAttributeName(AttributeMinified)]
        public bool UseMinified { get; set; }

        [HtmlAttributeNotBound]
        protected abstract bool AppendVersion { get; }

        [HtmlAttributeNotBound]
        protected abstract string MinExtension { get; }

        [HtmlAttributeNotBound]
        protected abstract string Extension { get; }

        protected BundleTagHelper(IApplicationEnvironment applicationEnvironment, IHostingEnvironment hostingEnvironment)
        {
            this.applicationEnvironment = applicationEnvironment;
            this.hostingEnvironment = hostingEnvironment;
        }

        protected abstract string BuildTag(string file);

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string file = File;

            if (!string.IsNullOrWhiteSpace(file))
            {
                file = HtmlFileHelper.ReplaceCultureTag(file);

                if (file.EndsWith(".bundle", StringComparison.OrdinalIgnoreCase))
                {
                    if (!UseBundle)
                    {
                        AppendBundleFiles(output, file);

                    }
                    else
                    {
                        AppendBundledFile(output, file);
                    }
                }
                else
                {
                    AppendSimpleFile(output, file);
                }

                output.TagName = string.Empty;
            }

            base.Process(context, output);
        }

        private void AppendBundleFiles(TagHelperOutput output, string file)
        {
            StringBuilder mvcHtmlStringBuilder = new StringBuilder();
            mvcHtmlStringBuilder.Append("<!-- BUNDLE START: ");
            mvcHtmlStringBuilder.Append(file);
            mvcHtmlStringBuilder.AppendLine("-->");

            int slashIndex = file.LastIndexOf('/');

            string directory = MakeAbsolutePath((slashIndex >= 0 ? file.Substring(0, slashIndex) : file).Trim('/'));

            IFileInfo bundleFile = hostingEnvironment.WebRootFileProvider.GetFileInfo(NormalizePath(file));

            if (bundleFile.Exists)
            {
                using (Stream stream = bundleFile.CreateReadStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string includedPath;

                        while ((includedPath = reader.ReadLine()) != null)
                        {
                            if (includedPath.StartsWith("//= include", StringComparison.OrdinalIgnoreCase))
                            {
                                includedPath = includedPath.Replace("//= include ", string.Empty);
                                includedPath = includedPath.TrimStart('/');

                                includedPath = directory + "/" + includedPath;

                                includedPath = AdjustContentPath(includedPath);

                                string tag = BuildTag(includedPath);

                                mvcHtmlStringBuilder.AppendLine(tag);
                            }
                        }
                    }
                }
            }

            mvcHtmlStringBuilder.Append("<!-- BUNDLE END: ");
            mvcHtmlStringBuilder.Append(file);
            mvcHtmlStringBuilder.AppendLine("-->");

            output.PostContent.SetContentEncoded(mvcHtmlStringBuilder.ToString());
        }

        private void AppendBundledFile(TagHelperOutput output, string file)
        {
            file = file.Replace(".bundle", Extension);
            file = AdjustContentPath(file);

            string tag = BuildTag(file);

            output.PostContent.SetContentEncoded(tag);
        }

        private void AppendSimpleFile(TagHelperOutput output, string file)
        {
            file = AdjustContentPath(file);

            string tag = BuildTag(file);

            output.PostContent.SetContentEncoded(tag);
        }

        protected internal virtual string NormalizePath(string path)
        {
            if (path.StartsWith("~/", StringComparison.Ordinal))
            {
                return path.Substring(1).Replace('\\', '/');
            }

            if (path.StartsWith("~\\", StringComparison.Ordinal))
            {
                return path;
            }

            return path.Replace('\\', '/');
        }

        private string AdjustContentPath(string path)
        {
            if (UseMinified && !string.IsNullOrWhiteSpace(MinExtension) && !path.EndsWith(MinExtension, StringComparison.OrdinalIgnoreCase))
            {
                path = path.Replace(Extension, MinExtension);
            }

            if (AppendVersion)
            {
                path += "?v=" + applicationEnvironment.ApplicationVersion;
            }

            path = MakeAbsolutePath(path);

            return path;
        }

        private string MakeAbsolutePath(string path)
        {
            string basePath = Base;

            if (string.IsNullOrWhiteSpace(basePath))
            {
                basePath = ViewContext.HttpContext.Request.PathBase;
            }

            if (!basePath.EndsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                basePath = basePath + "/";
            }

            path = basePath + path.TrimStart('/', '~').ToLowerInvariant();

            return path;
        }
    }
}
