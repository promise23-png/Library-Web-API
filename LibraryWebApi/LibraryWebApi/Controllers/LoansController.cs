using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryWebApi.Data;
using LibraryWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization; 
using System.ComponentModel.DataAnnotations;

namespace LibraryWebApi.Controllers
{
    [Route("api/[controller]")] 
    [ApiController] 
    [Authorize] 
    public class LoansController : ControllerBase
    {
        private readonly LibraryContext _context;

        
        public class IssueLoanDto
        {
            public int BookId { get; set; }
            public int BorrowerId { get; set; }
        }

        
        public class ReturnLoanDto
        {
            public int LoanId { get; set; }
        }

        public LoansController(LibraryContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Loan>>> GetLoans()
        {
            return await _context.Loans
                                 .Include(l => l.Book)
                                 .Include(l => l.Borrower)
                                 .ToListAsync();
        }

        // GET: api/Loans/5 - Endpoint to retrieve a single loan by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Loan>> GetLoan(int id)
        {
            var loan = await _context.Loans
                                     .Include(l => l.Book)
                                     .Include(l => l.Borrower)
                                     .FirstOrDefaultAsync(m => m.Id == id);

            if (loan == null)
            {
                return NotFound();
            }

            return loan;
        }

        // POST: api/Loans - Endpoint to issue a book (fulfills "issue a book" requirement)
        [HttpPost]
        public async Task<ActionResult<Loan>> IssueBook([FromBody] IssueLoanDto loanDto)
        {
            var book = await _context.Books.FindAsync(loanDto.BookId);
            if (book == null)
            {
                return NotFound(new { Message = $"Book with ID {loanDto.BookId} not found." });
            }
            if (book.AvailableCopies <= 0)
            {
                return BadRequest(new { Message = $"Book '{book.Title}' has no available copies to issue." });
            }

            var borrower = await _context.Borrowers.FindAsync(loanDto.BorrowerId);
            if (borrower == null)
            {
                return NotFound(new { Message = $"Borrower with ID {loanDto.BorrowerId} not found." });
            }

            // Decrement available copies
            book.AvailableCopies--;
            _context.Entry(book).State = EntityState.Modified; 

            // Create new Loan record
            var loan = new Loan
            {
                BookId = loanDto.BookId,
                BorrowerId = loanDto.BorrowerId,
                IssueDate = DateTime.UtcNow, 
                DueDate = DateTime.UtcNow.AddDays(14), 
                ReturnDate = null 
            };

            _context.Loans.Add(loan); 

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Error saving loan: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                return StatusCode(500, new { Message = "An error occurred while issuing the loan. Please try again.", Details = ex.Message });
            }

            return CreatedAtAction("GetLoan", new { id = loan.Id }, loan);
        }

        [HttpPost("returns")]
        public async Task<IActionResult> ReturnBook([FromBody] ReturnLoanDto returnDto) // Accept DTO for loanId
        {
            var loan = await _context.Loans
                                     .Include(l => l.Book) 
                                     .FirstOrDefaultAsync(l => l.Id == returnDto.LoanId && l.ReturnDate == null);

            if (loan == null)
            {
                return NotFound(new { Message = $"Active loan with ID {returnDto.LoanId} not found or already returned." });
            }

            loan.ReturnDate = DateTime.UtcNow;

            if (loan.Book != null)
            {
                loan.Book.AvailableCopies++;
            }
            else
            {
                return StatusCode(500, new { Message = "Book associated with loan not found during return process." });
            }

            try
            {
                await _context.SaveChangesAsync(); 
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Error saving return: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                return StatusCode(500, new { Message = "An error occurred while processing the return. Please try again.", Details = ex.Message });
            }

            return NoContent(); 
        }

        
        [HttpGet("overdue")]
        public async Task<ActionResult<IEnumerable<Loan>>> GetOverdueLoans()
        {
            var overdueLoans = await _context.Loans
                                             .Include(l => l.Book)    
                                             .Include(l => l.Borrower) 
                                             .Where(l => l.ReturnDate == null && l.DueDate < DateTime.UtcNow) // Filter for active and overdue loans
                                             .ToListAsync();

            return Ok(overdueLoans); 
        }

    }
}