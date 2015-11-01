// ==========================================================================
// HomeController.cs
// Green Parrot Framework
// ==========================================================================
// Copyright (c) Sebastian Stehle
// All rights reserved.
// ========================================================================== 

using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using SE.Upload.Web.Contracts;

namespace SE.Upload.Web.Controllers.Web
{
    [Route("")]
    public class HomeController : Controller
    {
        [FromServices]
        public IFileStorage FileStorage { get; set; }
        
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/{id}/")]
        public async Task<IActionResult> File(string id)
        {
            try
            {
                FileUpload file = await FileStorage.GetFileAsync(id);

                return View(file);
            }
            catch (FileNotFoundException)
            {
                return View("FileNotFound");
            }
            catch
            {
                return View("FileDeleted");
            }
        }
        
        [HttpGet("/download/{id}/")]
        public async Task<IActionResult> Download(string id)
        {
            try
            {
                FileUpload file = await FileStorage.GetFileAsync(id);

                byte[] content = Convert.FromBase64String(file.Content);

                return File(content, file.ContentType, file.Name);
            }
            catch (FileNotFoundException)
            {
                return View("FileNotFound");
            }
            catch
            {
                return View("FileDeleted");
            }
        }
        
        [HttpGet("legal/")]
        public IActionResult Legal()
        {
            return View();
        }
        
        [HttpGet("error/")]
        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}
