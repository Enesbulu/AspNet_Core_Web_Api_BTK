using Entities.Exceptions;
using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;

namespace Services.Manager
{
    public class BookManager : IBookService
    {
        private readonly IRepositoryManager _manager;
        private readonly ILoggerService _logger;

        public BookManager(IRepositoryManager manager, ILoggerService logger)
        {
            _manager = manager;
            _logger = logger;
        }

        public IEnumerable<Book> GetAllBooks(bool tractChanges)
        {
            return _manager.Book.GetAllBooks(tractChanges);
        }

        public Book GetOneBookById(int id, bool tractChanges)
        {
            var book = _manager.Book.GetOneBookById(id, tractChanges);
            if (book == null) throw new BookNotFoundException(id);
            return book;
        }

        public Book CreateOneBook(Book book)
        {
            _manager.Book.CreateOneBook(book);
            _manager.Save();
            return book;
        }

        public void UpdateOneBook(int id, Book book, bool tractChanges)
        {
            //check params
            if (book is null)
            {
                throw new ArgumentNullException(nameof(book));
            }
            //check entity
            var entity = _manager.Book.GetOneBookById(id, tractChanges);
            if (entity is null)
            {
                //string msg = $"Book with id: {id} could not Found";
                //_logger.LogInfo(msg);
                //throw new Exception($"Book with id: {id} could not Found");
                throw new BookNotFoundException(id);
            }

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
            {
                //    _logger.LogInfo($"The book with id: {id} could not found.");    //Bir alma ifadesi tanımlaması.
                //    throw new Exception($"Book with id {id} could not Found");

                throw new BookNotFoundException(id);
            }

            _manager.Book.Delete(entity);
            _manager.Save();
        }

    }
}
