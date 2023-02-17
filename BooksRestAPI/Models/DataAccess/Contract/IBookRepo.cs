using BooksRestAPI.Models.Entities;
using BooksRestAPI.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BooksRestAPI.Models.DataAccess.Contract
{
    public interface IBookRepo
    {
        Task<ActionResult<IEnumerable<Book>>> GetBooks();
        Task<ActionResult<Book>> GetBook(int id);
        Task<IActionResult> PutBook(int id, Book book);
        Task<ActionResult<Book>> PostBook(Book book);
        Task<IActionResult> DeleteBook(int id);
        void Save();
    }
}
