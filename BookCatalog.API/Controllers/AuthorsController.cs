using Microsoft.AspNetCore.Mvc;
using BookCatalog.API.Models;
using BookCatalog.API.Data;
using System.Linq;

namespace BookCatalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IDataStore _dataStore;

        public AuthorsController(IDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        [HttpGet]
        public IActionResult GetAuthors()
        {
            return Ok(_dataStore.Authors);
        }

        [HttpGet("{id}")]
        public IActionResult GetAuthor(int id)
        {
            var author = _dataStore.Authors.FirstOrDefault(a => a.Id == id);
            if (author == null) return NotFound();
            return Ok(author);
        }

        [HttpPost]
        public IActionResult CreateAuthor([FromBody] AuthorCreateDto authorDto)
        {
            var newId = _dataStore.Authors.Any() ? _dataStore.Authors.Max(a => a.Id) + 1 : 1;
            var author = new Author
            {
                Id = newId,
                Name = authorDto.Name,
                Surname = authorDto.Surname,
                BirthYear = authorDto.BirthYear
            };
            _dataStore.Authors.Add(author);
            return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAuthor(int id, [FromBody] Author author)
        {
            var existingAuthor = _dataStore.Authors.FirstOrDefault(a => a.Id == id);
            if (existingAuthor == null) return NotFound();

            existingAuthor.Name = author.Name;
            existingAuthor.Surname = author.Surname;
            existingAuthor.BirthYear = author.BirthYear;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAuthor(int id)
        {
            var author = _dataStore.Authors.FirstOrDefault(a => a.Id == id);
            if (author == null) return NotFound();

            var relatedBooks = _dataStore.Books.Any(b => b.Authors.Contains(author));
            if (relatedBooks) return BadRequest("Cannot delete author with related books.");

            _dataStore.Authors.Remove(author);
            return NoContent();
        }
    }
}
