using Microsoft.AspNetCore.Mvc;
using BankApp.Data;
using BankApp.Models;
using BankApp.ViewModels;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize( Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize( Roles = "Admin")]
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
        [AllowAnonymous]
        public IActionResult Details(int? id)
        {
            if ( id == null || id == 0 ) {
                return NotFound();
            }
            var productFromDb = _db.Products.Find(id);

            if ( productFromDb == null ) {
                return NotFound();
            }

            ViewBag.product = productFromDb;

            return View();
        }
        [Authorize( Roles = "Admin")]
        public IActionResult Update(int? id)
        {
            if ( id == null || id == 0 ) {
                return NotFound();
            }
            var productFromDb = _db.Products.Find(id);

            if ( productFromDb == null ) {
                return NotFound();
            }

            ViewBag.id = id;

            return View(productFromDb);
        }
        [HttpPost]
        [Authorize( Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Update(ProductViewModel model, int? id)
        {
            if ( ModelState.IsValid ) {
                string uniqueFileName = UploadedFile(model);

                var product = _db.Products.Find(id);
                product.Name = model.Name;
                product.Description = model.Description;
                product.Body = model.Body;

                if ( uniqueFileName != null ) {
                    product.ImagePath = uniqueFileName;
                }

                _db.Update(product);
                _db.SaveChanges();
                return RedirectToAction("Update");
            }
            return View();
        }
        [Authorize( Roles = "Admin")]
        public IActionResult Delete(int? id)
        {
            var obj = _db.Products.Find(id);
            if ( obj == null ) {
                return NotFound();
            }
            _db.Products.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index", "Home");
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