using Microsoft.AspNetCore.Mvc;
using BankApp.Data;
using BankApp.Models;
using BankApp.ViewModels;

namespace BankApp.Controllers
{
    public class OfferController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment webHostEnvironment;

        public OfferController(ApplicationDbContext db, IWebHostEnvironment hostEnvironment)
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
        public async Task<IActionResult> Create(OfferViewModel model)
        {
            if ( ModelState.IsValid ) {
                string uniqueFileName = UploadedFile(model);

                Offer offer = new Offer {
                    Name = model.Name,
                    Description = model.Description,
                    Body = model.Body,
                    ImagePath = uniqueFileName
                };

                _db.Add(offer);
                await _db.SaveChangesAsync();
                return RedirectToAction("Create");
            }
            return View();
        }

        private string UploadedFile(OfferViewModel model) {
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