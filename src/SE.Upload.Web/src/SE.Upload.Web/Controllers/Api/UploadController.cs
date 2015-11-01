// ==========================================================================
// UploadController.cs
// Green Parrot Framework
// ==========================================================================
// Copyright (c) Sebastian Stehle
// All rights reserved.
// ========================================================================== 

using System;
using System.Threading.Tasks;
using GP.Utils.Reflection;
using Microsoft.AspNet.Mvc;
using SE.Upload.Web.Contracts;
using SE.Upload.Web.Model;

namespace SE.Upload.Web.Controllers.Api
{
    [Route("api/upload")]
    public class UploadController : Controller
    {
        [FromServices]
        public IFileStorage FileStorage { get; set; }

        [HttpPost]
        public async Task<IActionResult> Upload([FromBody] UploadModel model)
        {
            FileUpload file = SimpleMapper.Map(model, new FileUpload());
            
            string id = await FileStorage.UploadAsync(file);

            return CreatedAtAction("File", "Home", new { id = id }, new { id = id });
        }
    }
}
