using System;
using System.ComponentModel.DataAnnotations; // Required for validation attributes

namespace LibraryWebApi.Models
{
    public class Loan
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Book ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Book ID must be a positive number.")]
        public int BookId { get; set; }
        public Book Book { get; set; } // Navigation property

        [Required(ErrorMessage = "Borrower ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Borrower ID must be a positive number.")]
        public int BorrowerId { get; set; }
        public Borrower Borrower { get; set; } // Navigation property

        [Required(ErrorMessage = "Issue date is required.")]
        public DateTime IssueDate { get; set; }

        [Required(ErrorMessage = "Due date is required.")]
        public DateTime DueDate { get; set; }

        public DateTime? ReturnDate { get; set; } // Nullable, as it might not be set initially
    }
}