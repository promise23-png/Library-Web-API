using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryWebApi.Data;     // Your DbContext namespace
using LibraryWebApi.Models;   // Your Borrower Model namespace (and others like Book, Loan if needed)
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace LibraryWebApi.Controllers
{
    [Route("api/[controller]")] // This means endpoints will start with /api/Borrowers
    [ApiController]
    [Authorize]
    public class BorrowersController : ControllerBase // <<< CHANGE 1: Class name
    {
        private readonly LibraryContext _context;

        public BorrowersController(LibraryContext context)
        {
            _context = context;
        }

        // GET: api/Borrowers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Borrower>>> GetBorrowers() // <<< CHANGE 2: Return type and method name
        {
            return await _context.Borrowers.ToListAsync(); // <<< CHANGE 3: Use _context.Borrowers
        }

        // GET: api/Borrowers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Borrower>> GetBorrower(int id) 
        {
            var borrower = await _context.Borrowers.FindAsync(id); 

            if (borrower == null) // <<< CHANGE 6: Variable name
            {
                return NotFound();
            }

            return borrower; // <<< CHANGE 7: Variable name
        }

        // POST: api/Borrowers
        [HttpPost]
        public async Task<ActionResult<Borrower>> PostBorrower(Borrower borrower) 
        {
            _context.Borrowers.Add(borrower); 
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBorrower", new { id = borrower.Id }, borrower); 
        }

        // PUT: api/Borrowers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBorrower(int id, Borrower borrower) 
        {
            if (id != borrower.Id) 
            {
                return BadRequest();
            }

            _context.Entry(borrower).State = EntityState.Modified; 

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BorrowerExists(id)) 
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Borrowers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBorrower(int id)
        {
            var borrower = await _context.Borrowers.FindAsync(id); 
            if (borrower == null) 
            {
                return NotFound();
            }

            _context.Borrowers.Remove(borrower);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Helper method to check if a borrower exists
        private bool BorrowerExists(int id) 
        {
            return _context.Borrowers.Any(e => e.Id == id); 
        }
    }
}