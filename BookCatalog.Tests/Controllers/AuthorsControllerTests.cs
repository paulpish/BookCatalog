using Xunit;
using Moq;
using BookCatalog.API.Controllers;
using BookCatalog.API.Data;
using BookCatalog.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BookCatalog.Tests.Controllers
{
    public class AuthorsControllerTests
    {
        private readonly Mock<IDataStore> _mockDataStore;
        private readonly AuthorsController _controller;

        public AuthorsControllerTests()
        {
            _mockDataStore = new Mock<IDataStore>();
            _controller = new AuthorsController(_mockDataStore.Object);
        }

        [Fact]
        public void GetAuthors_ReturnsOkResult_WithListOfAuthors()
        {
            // Arrange
            _mockDataStore.Setup(repo => repo.Authors).Returns(GetTestAuthors());

            // Act
            var result = _controller.GetAuthors();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var authors = Assert.IsAssignableFrom<IEnumerable<Author>>(okResult.Value);
            Assert.Equal(3, authors.Count());
        }

        [Fact]
        public void GetAuthor_ReturnsOkResult_WithAuthor()
        {
            // Arrange
            int testAuthorId = 1;
            _mockDataStore.Setup(repo => repo.Authors).Returns(GetTestAuthors());

            // Act
            var result = _controller.GetAuthor(testAuthorId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var author = Assert.IsAssignableFrom<Author>(okResult.Value);
            Assert.Equal(testAuthorId, author.Id);
        }

        [Fact]
        public void GetAuthor_ReturnsNotFoundResult_WhenAuthorDoesNotExist()
        {
            // Arrange
            int testAuthorId = 4;
            _mockDataStore.Setup(repo => repo.Authors).Returns(GetTestAuthors());

            // Act
            var result = _controller.GetAuthor(testAuthorId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void CreateAuthor_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var newAuthor = new AuthorCreateDto { Name = "New", Surname = "Author", BirthYear = 2000 };
            _mockDataStore.Setup(repo => repo.Authors).Returns(GetTestAuthors());

            // Act
            var result = _controller.CreateAuthor(newAuthor);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var author = Assert.IsAssignableFrom<Author>(createdAtActionResult.Value);
            Assert.Equal(4, author.Id); // Assuming the new author gets the next ID (4)
        }

        [Fact]
        public void DeleteAuthor_ReturnsBadRequest_WhenAuthorHasRelatedBooks()
        {
            // Arrange
            int testAuthorId = 1;
            var testAuthors = GetTestAuthors();
            var testBooks = GetTestBooks(testAuthors);
            _mockDataStore.Setup(repo => repo.Authors).Returns(testAuthors);
            _mockDataStore.Setup(repo => repo.Books).Returns(testBooks);

            // Act
            var result = _controller.DeleteAuthor(testAuthorId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        private List<Author> GetTestAuthors()
        {
            return new List<Author>
            {
                new Author { Id = 1, Name = "George", Surname = "Orwell", BirthYear = 1903 },
                new Author { Id = 2, Name = "Aldous", Surname = "Huxley", BirthYear = 1894 },
                new Author { Id = 3, Name = "J.K.", Surname = "Rowling", BirthYear = 1965 }
            };
        }

        private List<Book> GetTestBooks(List<Author> authors)
        {
            return new List<Book>
            {
                new Book
                {
                    Id = 1,
                    Title = "1984",
                    Authors = new List<Author> { authors[0] },
                    Publisher = "Secker & Warburg",
                    Edition = "1st",
                    PublishedDate = new System.DateTime(1949, 6, 8)
                }
            };
        }
    }
}
