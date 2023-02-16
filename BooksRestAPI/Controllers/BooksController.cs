using BooksRestAPI.Models;
using BooksRestAPI.Models.DataAccess.Contract;
using BooksRestAPI.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

            [HttpGet("{id}")]
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

