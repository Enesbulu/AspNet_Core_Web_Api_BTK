using Entities.Models;
using Entities.RequestFeatures;

namespace Repositories.Contracts
{
    public interface IBookRepository : IRepositoryBase<Book>
    {
        Task<PageList<Book>> GetAllBooksAsync(BookParametres bookParametres, bool trackChanges);
        Task<Book> GetOneBookByIdAsync(int id, bool trackChanges);
        void CreateOneBook(Book book);
        void UpdateOneBook(Book book);
        void DeleteOneBook(Book book);

    }
}
