using FileUpload.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FileUpload.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(IWebHostEnvironment webHostEnvironment)
        {
            this._webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AjaxUpload(List<IFormFile> files)
        {
            List<string> uploadedFilesNames = new List<string>();
            if (files != null && files.Count > 0)
            {
                foreach (IFormFile MyUploader in files)
                {
                    if (MyUploader != null)
                    {
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "AjaxMediaUpload");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }
                        string filePath = Path.Combine(uploadsFolder, MyUploader.FileName);
                        uploadedFilesNames.Add(MyUploader.FileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            MyUploader.CopyTo(fileStream);
                        }

                    }
                }
                return new ObjectResult(new { status = "success", filename = string.Join(",", uploadedFilesNames) });
            }
            return new ObjectResult(new { status = "fail" });

        }
        [HttpPost]
        public IActionResult FileUpload(IFormFile fileToBeUpload)
        {
            if (fileToBeUpload != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "HttpMediaUpload");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                string filePath = Path.Combine(uploadsFolder, fileToBeUpload.FileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    fileToBeUpload.CopyTo(fileStream);
                }
            }
            return RedirectToAction("Index");
        }

    }
}