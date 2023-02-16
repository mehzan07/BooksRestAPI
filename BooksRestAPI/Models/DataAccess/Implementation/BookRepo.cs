using BooksRestAPI.Models.DataAccess.Contract;
using BooksRestAPI.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BooksRestAPI.Models.DataAccess.Implementation
{

        public class BookRepo : IBookRepo
        {
            private readonly BookDBContext bookDBContext;
            public BookRepo(BookDBContext _bookDBContext)
            {
                bookDBContext = _bookDBContext;
            }

            public async Task<IActionResult> DeleteBook(int id)
            {
                var book = await bookDBContext.Books.FindAsync(id);
                if (book == null)
                {
                    return new NotFoundResult();
                }

                bookDBContext.Books.Remove(book);
                await bookDBContext.SaveChangesAsync();

                return new NoContentResult();
            }

            public async Task<ActionResult<Book>> GetBook(int id)
            {
                var book = await bookDBContext.Books.FindAsync(id);

                if (book == null)
                {
                    return new NotFoundResult();
                }

                return book;
            }

            public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
            {
                return await bookDBContext.Books.ToListAsync();
            }

            public async Task<ActionResult<Book>> PostBook(Book book)
            {
                bookDBContext.Books.Add(book);
                await bookDBContext.SaveChangesAsync();

                return book;
            }

            public async Task<IActionResult> PutBook(int id, Book book)
            {
                if (id != book.Id)
                {
                    return new BadRequestResult();
                }

                bookDBContext.Entry(book).State = EntityState.Modified;

                try
                {
                    await bookDBContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(id))
                    {
                        return new NotFoundResult();
                    }
                    else
                    {
                        throw;
                    }
                }

                return new NoContentResult();
            }
            private bool BookExists(int id)
            {
                return bookDBContext.Books.Any(e => e.Id == id);
            }
        }
    }
