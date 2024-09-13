using Entities.DataTransferObjects;
using Entities.Models;

namespace Services.Contracts
{
    public interface IBookService
    {
        IEnumerable<BookDto> GetAllBooks(bool trackChanges);
        BookDto GetOneBookById(int id, bool trackChanges);
        BookDto CreateOneBook(BookDtoForInsetion book);
        void UpdateOneBook(int id, BookDtoForUpdate bookDto, bool tractChanges);
        void DeleteOneBook(int id, bool trackChanges);
        (BookDtoForUpdate bookDtoForUpdate, Book book) GetOneBookForPatch(int id, bool trackChanges);
        void SaveChangesForPatch(BookDtoForUpdate bookDtoForUpdate, Book book);
    }
}
