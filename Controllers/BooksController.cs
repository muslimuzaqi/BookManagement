using BookMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookMVC.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext context;

        public BooksController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var books = context.Books.ToList();
            return View(books);
        }
    }
}
