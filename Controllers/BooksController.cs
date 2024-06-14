using BookMVC.Models;
using BookMVC.Services;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.AspNetCore.Mvc;

namespace BookMVC.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;

        public BooksController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }

        public IActionResult Index()
        {
            var books = context.Books.OrderByDescending(p => p.Id).ToList();
            return View(books);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(BookDto bookDto)
        {
            if (bookDto.ImageFileName == null)
            {
                ModelState.AddModelError("ImageFile", "The image file is required");
            }
            if (!ModelState.IsValid)
            {
                return View(bookDto);
            }

            //save the image file
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmmssfff");
            newFileName += Path.GetExtension(bookDto.ImageFileName!.FileName);

            string imageFullPath = environment.WebRootPath + "/books/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                bookDto.ImageFileName.CopyTo(stream);
            }

            //save the new book in the database

            Book book = new Book()
            {
                Title = bookDto.Title,
                Description = bookDto.Description,
                Author = bookDto.Author,
                Brand = bookDto.Brand,
                Category = bookDto.Category,
                Price = bookDto.Price,
                ImageFileName = newFileName,
                CreatedAt = DateTime.Now,
            };

            context.Books.Add(book);
            context.SaveChanges();


            return RedirectToAction("Index", "Books");
        }

        public IActionResult Edit(int id) 
        {
            var book = context.Books.Find(id);

            if (book == null)
            {
                return RedirectToAction("Index", "Books");
            }

            // create booksDTO from Book
            var bookDto = new BookDto()
            {
                Title = book.Title,
                Description = book.Description,
                Author = book.Author,
                Brand = book.Brand,
                Category = book.Category,
                Price = book.Price,
            };

            ViewData["BookId"] = book.Id;
            ViewData["ImageFileName"] = book.ImageFileName;
            ViewData["CreatedAt"] = book.CreatedAt.ToString("MM/dd/yyyy");

            return View(bookDto);
        }

        [HttpPost]
        public IActionResult Edit(int id, BookDto bookDto)
        {
            var book = context.Books.Find(id);

            if (book == null)
            {
                return RedirectToAction("Index", "Books");
            }

            if (!ModelState.IsValid)
            {
                ViewData["BookId"] = book.Id;
                ViewData["ImageFileName"] = book.ImageFileName;
                ViewData["CreatedAt"] = book.CreatedAt.ToString("MM/dd/yyyy");

                return View(bookDto);
            }

            //update the image file if we have  a new image file

            string newFileName = book.ImageFileName;
            if (bookDto.ImageFileName != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(bookDto.ImageFileName.FileName);

                string imageFullPath = environment.WebRootPath + "/books/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    bookDto.ImageFileName.CopyTo(stream);
                }

                // delete the old image
                string oldImageFullPath = environment.WebRootPath + "/books/" + bookDto.ImageFileName.FileName;
                System.IO.File.Delete(oldImageFullPath);   
            }

            //update the book in the database
            book.Title = bookDto.Title;
            book.Description = bookDto.Description;
            book.Author = bookDto.Author;
            book.Brand = bookDto.Brand;
            book.Price = bookDto.Price;
            book.Category = bookDto.Category;
            book.ImageFileName = newFileName;

            context.SaveChanges();

            return RedirectToAction("Index", "Books");
        }
            
        public IActionResult Delete(int id)
        {
            var book = context.Books.Find(id);
            if (book == null)
            {
                return RedirectToAction("Index", "Books");
            }
            string imageFullPath = environment.WebRootPath + "/books/" + book.ImageFileName;
            System.IO.File.Delete(imageFullPath);

            context.Books.Remove(book);
            context.SaveChanges(true);

            return RedirectToAction("Index", "Books");
        }
    }
}
