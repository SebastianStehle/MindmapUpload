// ==========================================================================
// FileUpload.cs
// Green Parrot Framework
// ==========================================================================
// Copyright (c) Sebastian Stehle
// All rights reserved.
// ========================================================================== 

using System;

namespace SE.Upload.Web.Contracts
{
    public class FileUpload
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Content { get; set; }

        public string ContentType { get; set; }

        public DateTime Uploaded { get; set; }
    }
}
