using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity; // If you use IdentityUser directly
using Microsoft.EntityFrameworkCore; // Needed for DbContext
using LibraryWebApi.Models; // Needed for your Book, Borrower, Loan models

namespace LibraryWebApi.Data
{
    // Change this line:
    public class LibraryContext : IdentityDbContext<Microsoft.AspNetCore.Identity.IdentityUser> // <<< MODIFIED
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Borrower> Borrowers { get; set; }
        public DbSet<Loan> Loans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // IMPORTANT: You MUST call the base OnModelCreating when using IdentityDbContext
            base.OnModelCreating(modelBuilder); // <<< ADD THIS LINE

            // Your existing model configurations below this
            modelBuilder.Entity<Loan>()
                .HasOne(l => l.Book)
                .WithMany()
                .HasForeignKey(l => l.BookId);

            modelBuilder.Entity<Loan>()
                .HasOne(l => l.Borrower)
                .WithMany()
                .HasForeignKey(l => l.BorrowerId);
        }
    }
}