using BookCatalog.API.Models;
using System.Collections.Generic;

namespace BookCatalog.API.Data
{
    public interface IDataStore
    {
        List<Author> Authors { get; set; }
        List<Book> Books { get; set; }
    }
}
