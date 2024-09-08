using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;

namespace Services.Manager
{
    public class BookManager : IBookService
    {
        private readonly IRepositoryManager _manager;

        public BookManager(IRepositoryManager manager)
        {
            _manager = manager;
        }

        public IEnumerable<Book> GetAllBooks(bool tractChanges)
        {
            return _manager.Book.GetAllBooks(tractChanges);
        }

        public Book GetOneBookById(int id, bool tractChanges)
        {
            return _manager.Book.GetOneBookById(id, tractChanges);
        }

        public Book CreateOneBook(Book book)
        {
            if (book is null)
                throw new ArgumentNullException(nameof(book));

            _manager.Book.CreateOneBook(book);
            _manager.Save();
            return book;
        }

        public void UpdateOneBook(int id, Book book, bool tractChanges)
        {
            //check params
            if (book is null)
                throw new ArgumentNullException(nameof(book));

            //check entity
            var entity = _manager.Book.GetOneBookById(id, tractChanges);
            if (entity is null)
                throw new Exception($"Book with id {id} could not Found");

            entity.Title = book.Title;
            entity.Price = book.Price;

            _manager.Book.Update(entity);
            _manager.Save();

        }

        public void DeleteOneBook(int id, bool tractChanges)
        {
            //check entity
            var entity = _manager.Book.GetOneBookById(id, tractChanges);
            if (entity is null)
                throw new Exception($"Book with id {id} could not Found");


            _manager.Book.Delete(entity);
            _manager.Save();
        }

    }
}
