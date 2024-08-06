using Microsoft.AspNetCore.Mvc;
using BookCatalog.API.Models;
using BookCatalog.API.Data;
using System.Linq;

namespace BookCatalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IDataStore _dataStore;

        public BooksController(IDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        [HttpGet]
        public IActionResult GetBooks()
        {
            return Ok(_dataStore.Books);
        }

        [HttpGet("{id}")]
        public IActionResult GetBook(int id)
        {
            var book = _dataStore.Books.FirstOrDefault(b => b.Id == id);
            if (book == null) return NotFound();
            return Ok(book);
        }

        [HttpPost]
        public IActionResult CreateBook([FromBody] BookCreateDto bookDto)
        {
            var newId = _dataStore.Books.Any() ? _dataStore.Books.Max(b => b.Id) + 1 : 1;
            var book = new Book
            {
                Id = newId,
                Title = bookDto.Title,
                Authors = bookDto.Authors,
                Publisher = bookDto.Publisher,
                Edition = bookDto.Edition,
                PublishedDate = bookDto.PublishedDate
            };
            _dataStore.Books.Add(book);
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, [FromBody] Book book)
        {
            var existingBook = _dataStore.Books.FirstOrDefault(b => b.Id == id);
            if (existingBook == null) return NotFound();

            existingBook.Title = book.Title;
            existingBook.Authors = book.Authors;
            existingBook.Publisher = book.Publisher;
            existingBook.Edition = book.Edition;
            existingBook.PublishedDate = book.PublishedDate;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            var book = _dataStore.Books.FirstOrDefault(b => b.Id == id);
            if (book == null) return NotFound();

            _dataStore.Books.Remove(book);
            return NoContent();
        }
    }
}
