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
        ///     Action to retrieve all Products.
        /// </summary>
        /// <returns>Returns a list of all Products or an empty list, if no Product is exist</returns>
        /// <response code="200">Returned if the list of Products was retrieved</response>
        /// <response code="400">Returned if the Products could not be retrieved</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        ///     Action Get a Product.
        /// </summary>
        /// <param name="productId">The productId is a Prouct which should be retaind from DB </param>
        /// <returns>Returns is OK </returns>
        /// <response code="200">Returned if the Products has been found and retained </response>
        /// <response code="400">Returned if the Prodcut could not be found to retaind with ProductId</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{productId}", Name = "Get")]
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
        ///  Action: Put to update a Product in the Database
        /// </summary>
        /// <param name="product">The product is a Prouct which should be updated in DB </param>
        /// <returns>Returns is OK </returns>
        /// <response code="200">Returned if the Product was updated </response>
        /// <response code="400">Returned if the Prodcut could not be found with Product.Id</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        /// <param name="product">Model to create a new Product</param>
        /// <returns>Returns the created product</returns>
        /// <response code="200">Returned if the product was created</response>
        /// <response code="400">Returned if the model couldn't be parsed or the product couldn't be saved</response>
        /// <response code="422">Returned when the validation failed</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
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
        ///   Action Delete: to delete a Product on the Database.
        /// </summary>
        /// <param name="productId">The productId is a Prouct which should be deleted from DB </param>
        /// <returns>Returns is OK </returns>
        /// <response code="200">Returned if the Product has been found and deleted </response>
        /// <response code="400">Returned if the Prodcut could not be found for  deletion with ProductId</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]  

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

