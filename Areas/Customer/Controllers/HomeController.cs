using BooksSpring2024_sec02.Data;
using BooksSpring2024_sec02.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace BooksSpring2024_sec02.Areas.Customer.Controllers
{

    [Area("Customer")]
    public class HomeController : Controller
    {
        private BooksDBContext _dbContext;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, BooksDBContext booksDBContext) //dependency injection after the comma here
        {
            _logger = logger;

            _dbContext = booksDBContext;
        }

        public IActionResult Index()
        {
            var listOfBooks = _dbContext.Books.Include(c => c.Category);

            return View(listOfBooks.ToList()); //you can delay the tolist to this part
        }

        public IActionResult Details(int id)
        {
            Book book = _dbContext.Books.Find(id);


            _dbContext.Books.Entry(book).Reference(b => b.Category).Load(); // loads the category

            var cart = new Cart
            {
                BookId = id,
                Book = book,
                Quantity = 1
            };

            return View(cart);


        }

        [HttpPost]
        [Authorize]
        public IActionResult Details(Cart cart)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);//fetches the user ID

            cart.UserId = userId;

            Cart existingCart = _dbContext.Carts.FirstOrDefault(c => c.UserId == userId && c.BookId == cart.BookId); // check if the cart matches the user ID, and check if the book id matches anything in the cart

            if (existingCart != null) //cart exists already
            {
                //update the cart
                existingCart.Quantity += cart.Quantity;
                _dbContext.Carts.Update(existingCart);
            }
            else
            {
                //add the new item in the carts table
                _dbContext.Carts.Add(cart);
            }


            _dbContext.SaveChanges();

            return RedirectToAction("Index");

        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
