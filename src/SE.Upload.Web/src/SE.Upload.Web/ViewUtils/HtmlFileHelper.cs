// ==========================================================================
// HtmlFileHelper.cs
// Green Parrot Framework
// ==========================================================================
// Copyright (c) Sebastian Stehle
// All rights reserved.
// ========================================================================== 

using System.Globalization;

namespace SE.Upload.Web.ViewUtils
{
    public static class HtmlFileHelper
    {
        public static string ReplaceCultureTag(string input)
        {
            input = ReplaceLongCultureInPath(input);
            input = ReplaceShortCultureInPath(input);

            return input;
        }

        private static string ReplaceLongCultureInPath(string path)
        {
            const string cultureTag = "{culture}";

            if (path.Contains(cultureTag))
            {
                path = path.Replace(cultureTag, CultureInfo.CurrentCulture.Name);
            }

            return path;
        }

        private static string ReplaceShortCultureInPath(string path)
        {
            const string cultureTag = "{shortculture}";

            if (path.Contains(cultureTag))
            {
                path = path.Replace(cultureTag, CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLowerInvariant());
            }

            return path;
        }
    }
}
