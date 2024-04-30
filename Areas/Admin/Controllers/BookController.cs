using BooksSpring2024_sec02.Data;
using BooksSpring2024_sec02.Models;
using BooksSpring2024_sec02.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BooksSpring2024_sec02.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class BookController : Controller
    {
        private BooksDBContext _dbContext;
        private IWebHostEnvironment _environment; //provides information about the web hosting environment and applications running in it, it's able to look into the project and find the path to images in the wwwroot folder. In order to work with this you have to inject it into the controller

        public BookController(BooksDBContext context, IWebHostEnvironment environment)
        {
            _dbContext = context; // passes the dbContext object to the instance variable. this is how you inject your database into the controller, called dependency injection
            _environment = environment; //passes the environment to the instance variable (injection)

        }

        public IActionResult Index()//list or fetch all objects (books)
        {
            var listOfBooks = _dbContext.Books.ToList();

            return View(listOfBooks);
        }

        [HttpGet]
        public IActionResult Create()
        {
            IEnumerable<SelectListItem> listOfCategories = _dbContext.Categories.ToList().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.CategoryId.ToString()
            });

            //PROJECTION: allows us to project a category object to a SelectListItem object, where the name of the category is used as the text and the CategoryID is used as the value of the SelectListItem

            //1. ViewBag: allows us to transfer some data from the controller to the view. Much more simple
            //ViewBag.ListOfCategories = listOfCategories;

            //2. ViewData: allows us to pass data from the controller to the view (not vice versa)
            //ViewData["ListOfCategoriesVD"] = listOfCategories;

            //3. ViewModel: Make an additional model folder in the solution explorer.  (2/1 lecture) ... more complex
            BookWithCategoriesVM bookWithCategoriesVMobj = new BookWithCategoriesVM();

            bookWithCategoriesVMobj.Book = new Book();

            bookWithCategoriesVMobj.ListOfCategories = listOfCategories;

            return View(bookWithCategoriesVMobj);

        }

        [HttpPost]
        public IActionResult Create(BookWithCategoriesVM BookWithCategoriesVMobj, IFormFile imgFile)
        {
            if (ModelState.IsValid)
            {
                string wwwrootPath = _environment.WebRootPath; //this simplifies finding the path to the webroot folder

                if (imgFile != null)
                {
                    using (var fileStream = new FileStream(Path.Combine(wwwrootPath, @"Images\BookImages\" + imgFile.FileName), FileMode.Create))
                    {

                        imgFile.CopyTo(fileStream);//saves the file in the specified folder

                    }

                    BookWithCategoriesVMobj.Book.imgUrl = @"\Images\BookImages\" + imgFile.FileName;


                }

                _dbContext.Books.Add(BookWithCategoriesVMobj.Book);
                _dbContext.SaveChanges();

                return RedirectToAction("Index", "Book");
            }

            return View(BookWithCategoriesVMobj);
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            Book book = _dbContext.Books.Find(id);

            IEnumerable<SelectListItem> listOfCategories = _dbContext.Categories.ToList().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.CategoryId.ToString()
            });

            BookWithCategoriesVM bookWithCategoriesVM = new BookWithCategoriesVM();

            bookWithCategoriesVM.Book = book;

            bookWithCategoriesVM.ListOfCategories = listOfCategories;

            return View(bookWithCategoriesVM);
        }

        [HttpPost]
        public IActionResult Edit(BookWithCategoriesVM bookWithCategoriesVM, IFormFile imgFile)
        {
            string wwwrootPath = _environment.WebRootPath;
            if (ModelState.IsValid)
            {
                
                if (imgFile != null) //is there a file to add?
                {
                    if(!string.IsNullOrEmpty(bookWithCategoriesVM.Book.imgUrl)) //do we have an existing file? if so, delete it
                    {
                        var oldImgPath = Path.Combine(wwwrootPath, bookWithCategoriesVM.Book.imgUrl.TrimStart('\\')); //trim start because the root path ends with a "\" and the imgURL starts with a "\"
                        //need to fix the issue with the wwwrootpath here

                        if(System.IO.File.Exists(oldImgPath))
                        {
                            System.IO.File.Delete(oldImgPath);

                        }

                    }

                    //if there is no existing file, or the existing file has been deleted, the new file is added
                    using (var fileStream = new FileStream(Path.Combine(wwwrootPath, @"Images\BookImages\" + imgFile.FileName), FileMode.Create))
                    {

                        imgFile.CopyTo(fileStream);//saves the file in the specified folder

                    }

                    bookWithCategoriesVM.Book.imgUrl = @"\Images\BookImages\" + imgFile.FileName;

                }

                _dbContext.Books.Update(bookWithCategoriesVM.Book);
                _dbContext.SaveChanges();

                return RedirectToAction("Index");


            }

            return View(bookWithCategoriesVM); //things did not work as expected

        }
        

    }
}
