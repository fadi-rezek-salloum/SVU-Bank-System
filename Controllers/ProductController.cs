using Microsoft.AspNetCore.Mvc;
using BankApp.Data;
using BankApp.Models;
using BankApp.ViewModels;

namespace BankApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment webHostEnvironment;
        public ProductController(ApplicationDbContext db, IWebHostEnvironment hostEnvironment)
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
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if ( ModelState.IsValid ) {
                string uniqueFileName = UploadedFile(model);

                Product product = new Product {
                    Name = model.Name,
                    Description = model.Description,
                    Body = model.Body,
                    ImagePath = uniqueFileName
                };

                _db.Add(product);
                await _db.SaveChangesAsync();
                return RedirectToAction("Create");
            }
            return View();
        }
        private string UploadedFile(ProductViewModel model) {
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