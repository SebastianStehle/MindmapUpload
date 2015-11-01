// ==========================================================================
// UploadModel.cs
// Green Parrot Framework
// ==========================================================================
// Copyright (c) Sebastian Stehle
// All rights reserved.
// ========================================================================== 

using System.ComponentModel.DataAnnotations;

namespace SE.Upload.Web.Model
{
    public class UploadModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Content { get; set; }
        
        [Required]
        public string ContentType { get; set; }
    }
}
