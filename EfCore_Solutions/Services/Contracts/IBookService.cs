using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;

namespace Services.Contracts
{
    public interface IBookService
    {
        Task<(IEnumerable<BookDto> books, MetaData metaData)> GetAllBooksAsync(BookParametres bookParametres, bool trackChanges);
        Task<BookDto> GetOneBooksIdAsync(int id, bool trackChanges);
        Task<BookDto> CreateOneBookAsync(BookDtoForInsetion book);
        Task UpdateOneBookAsync(int id, BookDtoForUpdate bookDto, bool tractChanges);
        Task DeleteOneBookAsync(int id, bool trackChanges);
        Task<(BookDtoForUpdate bookDtoForUpdate, Book book)> GetOneBookForPatchAsync(int id, bool trackChanges);
        Task SaveChangesForPatchAsync(BookDtoForUpdate bookDtoForUpdate, Book book);
    }
}
