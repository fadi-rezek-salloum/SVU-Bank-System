using Microsoft.AspNetCore.Mvc;
using BankApp.Data;
using BankApp.Models;

namespace BankApp.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _db;

    public HomeController(ApplicationDbContext db)
    {
        _db = db;
    }
    public IActionResult Index()
    {
        IEnumerable<Product> productsList = _db.Products;
        IEnumerable<Offer> offersList = _db.Offers;
        IEnumerable<Function> functionsList = _db.Functions;

        ViewBag.productsList = productsList;
        ViewBag.offersList = offersList;
        ViewBag.functionsList = functionsList;

        return View();
    }
}
