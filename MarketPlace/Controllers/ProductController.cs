using MarketPlace.Data;
using MarketPlace.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

[Authorize]
public class ProductController : Controller
{
    private readonly ApplicationDbContext _db;

    public ProductController(ApplicationDbContext db)
    {
        _db = db;
    }

    [AllowAnonymous]
    public IActionResult Index()
    {
        List<Product> products = _db.Products.Include(p => p.Category).ToList();
        return View(products);
    }

    [Authorize]
    public IActionResult Create()
    {
        ViewBag.Categories = _db.Categories.ToList();
        return View();
    }

    [Authorize]
    [HttpPost]
    public IActionResult Create(Product obj)
    {
        if (ModelState.IsValid)
        {
            _db.Products.Add(obj);
            _db.SaveChanges();
            TempData["success"] = "Category created successfully";
            return RedirectToAction("Index");
        }

        ViewBag.Categories = _db.Categories.ToList();
        return View(obj);
    }

    [Authorize]
    public IActionResult Edit(int? Id)
    {
        if (Id == null || Id == 0)
        {
            return NotFound();
        }

        ViewBag.Categories = _db.Categories.ToList();

        Product? categoryFromDb = _db.Products.Find(Id);

        if (categoryFromDb == null)
        {
            return NotFound();
        }

        return View(categoryFromDb);
    }

    [Authorize]
    [HttpPost]
    public IActionResult Edit(Product obj)
    {
        if (ModelState.IsValid)
        {
            _db.Products.Update(obj);
            _db.SaveChanges();
            TempData["success"] = "Category updated successfully";
            return RedirectToAction("Index");
        }

        ViewBag.Categories = _db.Categories.ToList();

        return View(obj);
    }

    [Authorize]
    public IActionResult Delete(int? Id)
    {
        if (Id == null || Id == 0)
        {
            return NotFound();
        }

        Product? categoryFromDb = _db.Products.Find(Id);

        if (categoryFromDb == null)
        {
            return NotFound();
        }

        return View(categoryFromDb);
    }

    [Authorize]
    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePOST(int? Id)
    {
        Product? obj = _db.Products.Find(Id);

        if (obj == null)
        {
            return NotFound();
        }

        _db.Products.Remove(obj);
        _db.SaveChanges();
        TempData["success"] = "Category deleted successfully";
        return RedirectToAction("Index");
    }

    [AllowAnonymous]
    public IActionResult Details(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        Product? obj = _db.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == id);

        if (obj == null)
        {
            return NotFound();
        }

        return View(obj);
    }
}
