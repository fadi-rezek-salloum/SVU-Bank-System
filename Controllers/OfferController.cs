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
        public IActionResult Details(int? id)
        {
            if ( id == null || id == 0 ) {
                return NotFound();
            }
            var offerFromDb = _db.Offers.Find(id);

            if ( offerFromDb == null ) {
                return NotFound();
            }

            ViewBag.offer = offerFromDb;

            return View();
        }

        public IActionResult Update(int? id)
        {
            if ( id == null || id == 0 ) {
                return NotFound();
            }
            var offerFromDb = _db.Offers.Find(id);

            if ( offerFromDb == null ) {
                return NotFound();
            }

            ViewBag.id = id;

            return View(offerFromDb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(OfferViewModel model, int? id)
        {
            if ( ModelState.IsValid ) {
                string uniqueFileName = UploadedFile(model);

                var offer = _db.Offers.Find(id);
                offer.Name = model.Name;
                offer.Description = model.Description;
                offer.Body = model.Body;

                if ( uniqueFileName != null ) {
                    offer.ImagePath = uniqueFileName;
                }

                _db.Update(offer);
                _db.SaveChanges();
                return RedirectToAction("Update");
            }
            return View();
        }
        public IActionResult Delete(int? id)
        {
            var obj = _db.Offers.Find(id);
            if ( obj == null ) {
                return NotFound();
            }
            _db.Offers.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index", "Home");
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