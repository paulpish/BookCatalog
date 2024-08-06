using BookCatalog.API.Models;
using System.Collections.Generic;

namespace BookCatalog.API.Data
{
    public class InMemoryDataStore : IDataStore
    {
        public List<Author> Authors { get; set; } = new List<Author>();
        public List<Book> Books { get; set; } = new List<Book>();

        public InMemoryDataStore()
        {
            // Seed data
            Authors = new List<Author>
            {
                new Author { Id = 1, Name = "George", Surname = "Orwell", BirthYear = 1903 },
                new Author { Id = 2, Name = "Aldous", Surname = "Huxley", BirthYear = 1894 },
                new Author { Id = 3, Name = "J.K.", Surname = "Rowling", BirthYear = 1965 }
            };

            Books = new List<Book>
            {
                new Book
                {
                    Id = 1,
                    Title = "1984",
                    Authors = new List<Author> { Authors[0] },
                    Publisher = "Secker & Warburg",
                    Edition = "1st",
                    PublishedDate = new System.DateTime(1949, 6, 8)
                },
                new Book
                {
                    Id = 2,
                    Title = "Brave New World",
                    Authors = new List<Author> { Authors[1] },
                    Publisher = "Chatto & Windus",
                    Edition = "1st",
                    PublishedDate = new System.DateTime(1932, 8, 30)
                },
                new Book
                {
                    Id = 3,
                    Title = "Harry Potter and the Philosopher's Stone",
                    Authors = new List<Author> { Authors[2] },
                    Publisher = "Bloomsbury",
                    Edition = "1st",
                    PublishedDate = new System.DateTime(1997, 6, 26)
                }
            };
        }
    }
}
