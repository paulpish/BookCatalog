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
    public class BooksControllerTests
    {
        private readonly Mock<IDataStore> _mockDataStore;
        private readonly BooksController _controller;

        public BooksControllerTests()
        {
            _mockDataStore = new Mock<IDataStore>();
            _controller = new BooksController(_mockDataStore.Object);
        }

        [Fact]
        public void GetBooks_ReturnsOkResult_WithListOfBooks()
        {
            // Arrange
            _mockDataStore.Setup(repo => repo.Books).Returns(GetTestBooks());

            // Act
            var result = _controller.GetBooks();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var books = Assert.IsAssignableFrom<IEnumerable<Book>>(okResult.Value);
            Assert.Equal(3, books.Count());
        }

        [Fact]
        public void GetBook_ReturnsOkResult_WithBook()
        {
            // Arrange
            int testBookId = 1;
            _mockDataStore.Setup(repo => repo.Books).Returns(GetTestBooks());

            // Act
            var result = _controller.GetBook(testBookId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var book = Assert.IsAssignableFrom<Book>(okResult.Value);
            Assert.Equal(testBookId, book.Id);
        }

        [Fact]
        public void GetBook_ReturnsNotFoundResult_WhenBookDoesNotExist()
        {
            // Arrange
            int testBookId = 4;
            _mockDataStore.Setup(repo => repo.Books).Returns(GetTestBooks());

            // Act
            var result = _controller.GetBook(testBookId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void CreateBook_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var newBook = new BookCreateDto
            {
                Title = "New Book",
                Authors = new List<Author> { new Author { Id = 1, Name = "George", Surname = "Orwell", BirthYear = 1903 } },
                Publisher = "New Publisher",
                Edition = "1st",
                PublishedDate = new System.DateTime(2021, 1, 1)
            };
            _mockDataStore.Setup(repo => repo.Books).Returns(GetTestBooks());

            // Act
            var result = _controller.CreateBook(newBook);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var book = Assert.IsAssignableFrom<Book>(createdAtActionResult.Value);
            Assert.Equal(4, book.Id); // Assuming the new book gets the next ID (4)
        }

        [Fact]
        public void DeleteBook_ReturnsNoContentResult_WhenBookExists()
        {
            // Arrange
            int testBookId = 1;
            var testBooks = GetTestBooks();
            _mockDataStore.Setup(repo => repo.Books).Returns(testBooks);

            // Act
            var result = _controller.DeleteBook(testBookId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        private List<Book> GetTestBooks()
        {
            var authors = new List<Author>
            {
                new Author { Id = 1, Name = "George", Surname = "Orwell", BirthYear = 1903 },
                new Author { Id = 2, Name = "Aldous", Surname = "Huxley", BirthYear = 1894 },
                new Author { Id = 3, Name = "J.K.", Surname = "Rowling", BirthYear = 1965 }
            };

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
                },
                new Book
                {
                    Id = 2,
                    Title = "Brave New World",
                    Authors = new List<Author> { authors[1] },
                    Publisher = "Chatto & Windus",
                    Edition = "1st",
                    PublishedDate = new System.DateTime(1932, 8, 30)
                },
                new Book
                {
                    Id = 3,
                    Title = "Harry Potter and the Philosopher's Stone",
                    Authors = new List<Author> { authors[2] },
                    Publisher = "Bloomsbury",
                    Edition = "1st",
                    PublishedDate = new System.DateTime(1997, 6, 26)
                }
            };
        }
    }
}
