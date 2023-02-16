
using BooksRestAPI.Models.Entities;
using BooksRestAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BooksRestAPI.Models
{
    public class BookDBContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public BookDBContext(DbContextOptions<BookDBContext> options) : base(options)
        {

        }
    }
}
