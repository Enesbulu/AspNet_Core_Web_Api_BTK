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

        public async Task<IEnumerable<BookDto>> GetAllBooksAsync(bool tractChanges)
        {
            IEnumerable<Book> books = await _manager.Book.GetAllBooksAsync(tractChanges);

            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public async Task<BookDto> GetOneBooksIdAsync(int id, bool tractChanges)
        {
            Book book =await _manager.Book!.GetOneBookByIdAsync(id, tractChanges)!;
            if (book == null) throw new BookNotFoundException(id);
            return _mapper.Map<BookDto>(book);
        }

        public async Task<BookDto> CreateOneBookAsync(BookDtoForInsetion bookDto)
        {
            var entity = _mapper.Map<Book>(bookDto);
            _manager.Book.CreateOneBook(entity);
            await _manager.SaveAsync();
            return _mapper.Map<BookDto>(entity);
        }

        public async Task UpdateOneBookAsync(int id, BookDtoForUpdate bookDto, bool tractChanges)
        {
            //check entity
            var entity = await _manager!.Book!.GetOneBookByIdAsync(id, tractChanges)!;
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
            await _manager.SaveAsync();
        }

        public async Task DeleteOneBookAsync(int id, bool tractChanges)
        {
            //check entity
            var entity = await _manager.Book.GetOneBookByIdAsync(id, tractChanges);
            if (entity is null)
            {
                //    _logger.LogInfo($"The book with id: {id} could not found.");    //Bir alma ifadesi tanımlaması.
                //    throw new Exception($"Book with id {id} could not Found");

                throw new BookNotFoundException(id);
            }

            _manager.Book.Delete(entity);
            await _manager.SaveAsync();
        }

        public async Task<(BookDtoForUpdate bookDtoForUpdate, Book book)> GetOneBookForPatchAsync(int id, bool trackChanges)
        {
            var book = await _manager.Book.GetOneBookByIdAsync(id: id, trackChanges: trackChanges);

            if (book is null)
                throw new BookNotFoundException(id);

            var bookDtoForUpdate = _mapper.Map<BookDtoForUpdate>(book);

            return (bookDtoForUpdate, book);

        }

        public async Task SaveChangesForPatchAsync(BookDtoForUpdate bookDtoForUpdate, Book book)
        {
            _mapper.Map(bookDtoForUpdate, book);
            await _manager.SaveAsync();
        }
    }
}
