using BooksRestAPI.Models;
using BooksRestAPI.Models.DataAccess.Contract;
using BooksRestAPI.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BooksRestAPI.Controllers
{
   
        [Route("api/[controller]")]
        [ApiController]
        public class BooksController : ControllerBase
        {
            private readonly IBookRepo bookRepo;

            public BooksController(IBookRepo _bookRepo)
            {
                bookRepo = _bookRepo;
            }

        /// <summary>
        ///     Action to retrieve all Books.
        /// </summary>
        /// <returns>Returns a list of all Books or an empty list, if no Book is exist</returns>
        /// <response code="200">Returned if the list of Products was retrieved</response>
        /// <response code="400">Returned if the Products could not be retrieved</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
            {
                try
                {
                    return await bookRepo.GetBooks();
                }
                catch
                {
                    return StatusCode(500);
                }
            }

        /// <summary>
        ///     Action Get a Book.
        /// </summary>
        /// <param name="productId">The productId is a Prouct which should be retaind from DB </param>
        /// <returns>Returns is OK </returns>
        /// <response code="200">Returned if the Book has been found and retained </response>
        /// <response code="400">Returned if the Book could not be found to retaind with BookId</response>
        [HttpGet("{Id}", Name = "Get")]
        public async Task<ActionResult<Book>> GetBook(int id)
            {
                try
                {
                    var book = await bookRepo.GetBook(id);
                    return book;
                }
                catch
                {
                    return StatusCode(500);
                }
            }

        /// <summary>
        ///  Action: Put to update a book in the Database
        /// </summary>
        /// <param name="product">The Book is a book which should be updated in DB </param>
        /// <returns>Returns is OK </returns>
        /// <response code="200">Returned if the Book was updated </response>
        /// <response code="400">Returned if the book could not be found with book.Id</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
            {
                try
                {
                    var result = await bookRepo.PutBook(id, book);
                    return result;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return StatusCode(409);
                }
                catch (Exception ex)
                {
                    return StatusCode(500);
                }
            }


        /// <summary>
        /// Action: Post to create a new product in the database.
        /// </summary>
        /// <param name="book">Model to create a new book</param>
        /// <returns>Returns the created book</returns>
        /// <response code="200">Returned if the book was created</response>
        /// <response code="400">Returned if the model couldn't be parsed or the book couldn't be saved</response>
        /// <response code="422">Returned when the validation failed</response>
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
            {
                try
                {
                    var result = await bookRepo.PostBook(book);
                    return CreatedAtAction("GetBook", new { id = result.Value.Id }, book);
                }
                catch
                {
                    return StatusCode(500);
                }
            }

        /// <summary>
        ///   Action Delete: to delete a book on the Database.
        /// </summary>
        /// <param name="productId">The bookId is a book id which should be deleted from DB </param>
        /// <returns>Returns is OK </returns>
        /// <response code="200">Returned if the book id has been found and deleted </response>
        /// <response code="400">Returned if the book could not be found for  deletion with bookId</response>

        [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteBook(int id)
            {
                try
                {
                    var result = await bookRepo.DeleteBook(id);
                    return result;
                }
                catch
                {
                    return StatusCode(500);
                }
            }


        }
    }

