using Microsoft.AspNetCore.Mvc; // For [ApiController], ControllerBase, ActionResult
using Microsoft.EntityFrameworkCore; // For ToListAsync, FindAsync, etc.
using LibraryWebApi.Data; // Your DbContext
using LibraryWebApi.Models; // Your Book Model
using System.Collections.Generic; // For IEnumerable
using System.Linq; // For Any
using System.Threading.Tasks; // For async/await
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;


namespace LibraryWebApi.Controllers
{
    [Route("api/[controller]")] // This means endpoints will start with /api/Books
    [ApiController]
    [Authorize] 
    public class BooksController : ControllerBase
    {
        private readonly LibraryContext _context; 
        private readonly ILogger<BooksController> _logger;
        public BooksController(LibraryContext context, ILogger<BooksController> logger) 
        {
            _context = context;
            _logger = logger; // Assign logger
        }
    
        // GET: api/Books
        // Returns a list of all books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            _logger.LogInformation("Getting all books.");
            return await _context.Books.ToListAsync();
        }

        // GET: api/Books/5
        // Returns a single book by its ID
        [HttpGet("{id}")] // {id} is a placeholder for the book's ID in the URL
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _context.Books.FindAsync(id); // Find the book by its primary key

            if (book == null)
            {
                return NotFound(); // HTTP 404 Not Found if book doesn't exist
            }

            return book; // HTTP 200 OK with the book data
        }

        // POST: api/Books
        // Creates a new book
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book) // The book object is sent in the request body
        {
            _context.Books.Add(book); // Add the new book to the database context
            await _context.SaveChangesAsync(); // Save changes to the actual database

            // Return a 201 Created status, with the location of the newly created resource
            return CreatedAtAction("GetBook", new { id = book.Id }, book);
        }

        // PUT: api/Books/5
        // Updates an existing book
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.Id)
            {
                return BadRequest(); // HTTP 400 Bad Request if ID in URL doesn't match the book object's ID
            }

            _context.Entry(book).State = EntityState.Modified; // Tell EF Core that this entity has been modified

            try
            {
                await _context.SaveChangesAsync(); // Save changes
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id)) // Check if the book was deleted by another process
                {
                    return NotFound();
                }
                else
                {
                    throw; // Re-throw other types of concurrency errors
                }
            }

            return NoContent(); // HTTP 204 No Content (successful update, no data returned)
        }

        // DELETE: api/Books/5
        // Deletes a book by its ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound(); // HTTP 404 if book not found
            }

            _context.Books.Remove(book); // Remove the book from the context
            await _context.SaveChangesAsync(); // Save changes to the database

            return NoContent(); // HTTP 204 No Content
        }

        // Helper method to check if a book exists (used internally)
        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}