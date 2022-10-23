using Microsoft.AspNetCore.Mvc;
using BankApp.Data;
using BankApp.Models;
using BankApp.ViewModels;

namespace BankApp.Controllers
{
    public class FunctionController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment webHostEnvironment;

        public FunctionController(ApplicationDbContext db, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            webHostEnvironment = hostEnvironment;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FunctionViewModel model)
        {
            if ( ModelState.IsValid ) {
                string uniqueFileName = UploadedFile(model);

                Function function = new Function {
                    Name = model.Name,
                    Icon = model.Icon,
                    Body = model.Body,
                    ImagePath = uniqueFileName
                };

                _db.Add(function);
                await _db.SaveChangesAsync();
                return RedirectToAction("Create");
            }
            return View();
        }
        private string UploadedFile(FunctionViewModel model) {
            string uniqueFileName = null;
            if ( model.ImagePath != null ) {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "uploads");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImagePath.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using ( var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ImagePath.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }
    }
}