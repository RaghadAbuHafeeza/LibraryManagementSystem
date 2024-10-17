using LMS_LibraryManagementSystem_.Data; 
using LMS_LibraryManagementSystem_.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMS_LibraryManagementSystem_.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext context;

        // Used Dependency Injection to pass ApplicationDbContext
        public BooksController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var books = await context.Books.ToListAsync();

            return View(books);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var book = new Book();  // Initialize a new Book model(Create a new object of type Book)
            return View(book); 
        }

        [HttpPost]
        public async Task<IActionResult> Create(Book book)
        {
            // Check if there is a book with the same title
            if (context.Books.Any(b => b.Title == book.Title))
            {
                ModelState.AddModelError("Title", "The book already exists.");
                return View(book);
            }

            if (ModelState.IsValid)
            {
                context.Books.Add(book);
                await context.SaveChangesAsync();
                return RedirectToAction("Index");   
            }
            return View(book);
        }

        public async Task<IActionResult> Details(int id)
        {
            var book = await context.Books.FindAsync(id);
            if (book is null)
            {
                return NotFound();  
            }

            return View(book); 
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var book = await context.Books.FindAsync(id);
            if (book is null)
            {
                return NotFound();
            }
             return View("Create", book);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Book book)
        {
            if (context.Books.Any(b => b.Title == book.Title && b.Id != book.Id))
            {
                ModelState.AddModelError("Title", "The book already exists.");  
                return View(book);
            }
            if (id != book.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(book);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;  
                }

                return RedirectToAction(nameof(Index));
            }
            // If the model is not valid, return the same edit page to display the errors
            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await context.Books.FindAsync(id);
            if (book is null)
            {
                return NotFound();
            }

            context.Books.Remove(book);
            await context.SaveChangesAsync();

            return Ok();
        }
    }
}
