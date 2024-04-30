using BooksSpring2024_sec02.Data;
using BooksSpring2024_sec02.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BooksSpring2024_sec02.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = "Admin,Employee")]
    public class CategoryController : Controller
    {
        private BooksDBContext _dbContext;

        public CategoryController(BooksDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()//list or fetch all objects (categories)
        {
            var listOfCategories = _dbContext.Categories.ToList();

            return View(listOfCategories);
        }

        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]

        public IActionResult Create(Category categoryObj)
        {
            //custom validation
            if (categoryObj.Name != null && categoryObj.Name.ToLower() == "test")
            {
                ModelState.AddModelError("Name", "Category name cannot be 'test'");
            }

            //custom validation to make sure that name and description values are not exactly the same
            if (categoryObj.Name == categoryObj.Description)
            {
                ModelState.AddModelError("Description", "Category name and description cannot be the same");
            }


            if (ModelState.IsValid)
            {
                _dbContext.Categories.Add(categoryObj);
                _dbContext.SaveChanges();

                return RedirectToAction("Index", "Category");
            }

            return View(categoryObj);
        }



        public IActionResult Edit(int id)
        {
            Category category = _dbContext.Categories.Find(id);

            return View(category);
        }

        [HttpPost]

        public IActionResult Edit(int id, [Bind("CategoryID, Name, Description")] Category categoryObj)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Categories.Update(categoryObj);
                _dbContext.SaveChanges();

                return RedirectToAction("Index", "Category");
            }

            return View(categoryObj);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Category category = _dbContext.Categories.Find(id);

            return View(category);
        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeletePOST(int id)
        {
            Category categoryObj = _dbContext.Categories.Find(id);

            _dbContext.Categories.Remove(categoryObj);
            _dbContext.SaveChanges();

            return RedirectToAction("Index", "Category");
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            Category categoryObj = _dbContext.Categories.Find(id); //fetches the record

            return View(categoryObj);
        }


    }
}
