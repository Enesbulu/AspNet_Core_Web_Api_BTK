using AutoMapper;
using Entities.DataTransferObjects;
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
        private readonly IMapper _mapper;

        public BookManager(IRepositoryManager manager, ILoggerService logger, IMapper mapper)
        {
            _manager = manager;
            _logger = logger;
            _mapper = mapper;
        }

        public IEnumerable<BookDto> GetAllBooks(bool tractChanges)
        {
            IQueryable<Book> books = _manager.Book.GetAllBooks(tractChanges);

            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public BookDto GetOneBookById(int id, bool tractChanges)
        {
            var book = _manager.Book.GetOneBookById(id, tractChanges);
            if (book == null) throw new BookNotFoundException(id);
            return _mapper.Map<BookDto>(book);
        }

        public BookDto CreateOneBook(BookDtoForInsetion bookDto)
        {
            var entity = _mapper.Map<Book>(bookDto);
            _manager.Book.CreateOneBook(entity);
            _manager.Save();
            return _mapper.Map<BookDto>(entity);
        }

        public void UpdateOneBook(int id, BookDtoForUpdate bookDto, bool tractChanges)
        {
            //check entity
            var entity = _manager.Book.GetOneBookById(id, tractChanges);
            if (entity is null)
            {
                //string msg = $"Book with id: {id} could not Found";
                //_logger.LogInfo(msg);
                //throw new Exception($"Book with id: {id} could not Found");
                throw new BookNotFoundException(id);
            }

            //Mapping
            //entity.Title = book.Title;
            //entity.Price = book.Price;
            entity = _mapper.Map<Book>(bookDto);

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

        public (BookDtoForUpdate bookDtoForUpdate, Book book) GetOneBookForPatch(int id, bool trackChanges)
        {
            var book = _manager.Book.GetOneBookById(id: id, trackChanges: trackChanges);

            if (book is null)
                throw new BookNotFoundException(id);

            var bookDtoForUpdate = _mapper.Map<BookDtoForUpdate>(book);

            return (bookDtoForUpdate, book);

        }

        public void SaveChangesForPatch(BookDtoForUpdate bookDtoForUpdate, Book book)
        {
            _mapper.Map(bookDtoForUpdate, book);
            _manager.Save();
        }
    }
}
