using BulkyBookWeb.Data;
using BulkyBookWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly DatabaseContext _db;
        public CategoryController(DatabaseContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _db.Categories.ToList();
            return View(objCategoryList);
        }
        //Get
        public IActionResult Create()
        {

            return View();
        }
        //Post
        [HttpPost] //for post method
        [ValidateAntiForgeryToken] //help & prevent the cross site request forgery attack
        public IActionResult Create(Category obj)
        {
            //custom validation
            if (obj.DisplayOrder == 0)
            {
                ModelState.AddModelError("CustomError", "Check display order.");  //CustomError: key (unique) => display in asp-validation-summary, add custom error for property in table: key=name's property 
            }

            ModelState.Remove("DisplayOrder");
            if (ModelState.IsValid)
            {
                _db.Categories.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index"); //other controller RedirectToAction("page","controller")}
            }
            return View(obj);
        }
        //Get
        public IActionResult Edit(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }
            var category = _db.Categories.Find(id); //Find, FirstOrDefault, SingleOrDefault
            if(category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index"); 
            }
            return View(obj);
        }
        //Get
        public IActionResult Delete(int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }
            var category = _db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        //Post
        [HttpPost, ActionName("Delete")] //Use ActionName when action in form have different name from the method
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Category obj)
        {
            _db.Categories.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
    }
}

//TempData: store data and stay there for 1 request (use to alert, toast,...)