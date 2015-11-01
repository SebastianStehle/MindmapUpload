// ==========================================================================
// HomeControllerTests.cs
// Green Parrot Framework
// ==========================================================================
// Copyright (c) Sebastian Stehle
// All rights reserved.
// ========================================================================== 

using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Rhino.Mocks;
using SE.Upload.Web.Contracts;
using SE.Upload.Web.Controllers.Web;
using Xunit;

namespace SE.Upload.Web.Tests
{
    public class HomeControllerTests
    {
        private readonly HomeController homeController = new HomeController();
        private readonly IFileStorage storage;
        private readonly MockRepository mocks = new MockRepository();

        public HomeControllerTests()
        {
            storage = mocks.StrictMock<IFileStorage>();
        }

        [Fact]
        public async Task File_ShouldReturn_View()
        {
            string id = "<ID>";

            FileUpload model = new FileUpload();

            using (mocks.Record())
            {
                storage.Expect(x => x.GetFileAsync(id)).Return(Task.FromResult(model));
            }

            IActionResult result = await homeController.File(id);

            ViewResult viewResult = result as ViewResult;

            Assert.NotNull(viewResult);
            Assert.Equal(model, viewResult?.ViewData.Model);
        }

        [Fact]
        public void Index_ShouldReturn_View()
        {
            IActionResult result = homeController.Index();

            Assert.NotNull(result as ViewResult);
        }

        [Fact]
        public void Error_ShouldReturn_View()
        {
            IActionResult result = homeController.Error();

            Assert.NotNull(result as ViewResult);
        }

        [Fact]
        public void Legal_ShouldReturn_View()
        {
            IActionResult result = homeController.Legal();

            Assert.NotNull(result as ViewResult);
        }
    }
}
