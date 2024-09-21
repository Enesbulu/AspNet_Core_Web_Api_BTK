using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Exceptions;
using Entities.Models;
using Entities.RequestFeatures;
using Repositories.Contracts;
using Services.Contracts;
using System.Dynamic;

namespace Services.Manager
{
    public class BookManager : IBookService
    {
        private readonly IRepositoryManager _manager;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        private readonly IDataShaper<BookDto> _shaper;

        public BookManager(IRepositoryManager manager, ILoggerService logger, IMapper mapper, IDataShaper<BookDto> shaper)
        {
            _manager = manager;
            _logger = logger;
            _mapper = mapper;
            _shaper = shaper;
        }

        public async Task<(IEnumerable<ExpandoObject> books, MetaData metaData)> GetAllBooksAsync(BookParametres bookParametres, bool tractChanges)
        {
            if (!bookParametres.ValidPriceRange)
                throw new PriceOutofRangeBadRequestException(); //verilen fiyat aralığı geçerli değilse hata fırlatır.

            var booksWithMetaData = await _manager.Book.GetAllBooksAsync(bookParametres, tractChanges);
            var booksDto = _mapper.Map<IEnumerable<BookDto>>(booksWithMetaData);

            var shapedData = _shaper.ShapeData(booksDto, bookParametres.Fields);
            return (books:shapedData, metaData:booksWithMetaData.MetaData);
        }

        public async Task<BookDto> GetOneBooksIdAsync(int id, bool trackChanges)
        {
            //Book book = await _manager.Book!.GetOneBookByIdAsync(id, trackChanges)!;
            //if (book == null) throw new BookNotFoundException(id);
            var book = await GetOneBookByIdAndChechExits(id, trackChanges);
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
            //var entity = await _manager!.Book!.GetOneBookByIdAsync(id, tractChanges)!;
            //if (entity is null)
            //{
            //    //string msg = $"Book with id: {id} could not Found";
            //    //_logger.LogInfo(msg);
            //    //throw new Exception($"Book with id: {id} could not Found");
            //    throw new BookNotFoundException(id);
            //}
            var entity = await GetOneBookByIdAndChechExits(id, tractChanges);

            entity = _mapper.Map<Book>(bookDto);

            _manager.Book.Update(entity);
            await _manager.SaveAsync();
        }

        public async Task DeleteOneBookAsync(int id, bool tractChanges)
        {
            //check entity
            //var entity = await _manager.Book.GetOneBookByIdAsync(id, tractChanges);
            //if (entity is null)
            //{
            //    //    _logger.LogInfo($"The book with id: {id} could not found.");    //Bir alma ifadesi tanımlaması.
            //    //    throw new Exception($"Book with id {id} could not Found");
            //    throw new BookNotFoundException(id);
            //}
            var entity = await GetOneBookByIdAndChechExits(id: id, trackChanges: tractChanges);

            _manager.Book.Delete(entity);
            await _manager.SaveAsync();
        }

        public async Task<(BookDtoForUpdate bookDtoForUpdate, Book book)> GetOneBookForPatchAsync(int id, bool trackChanges)
        {
            var book = await GetOneBookByIdAndChechExits(id: id, trackChanges: trackChanges);

            //if (book is null)
            //    throw new BookNotFoundException(id);

            var bookDtoForUpdate = _mapper.Map<BookDtoForUpdate>(book);

            return (bookDtoForUpdate, book);

        }

        public async Task SaveChangesForPatchAsync(BookDtoForUpdate bookDtoForUpdate, Book book)
        {
            _mapper.Map(bookDtoForUpdate, book);
            await _manager.SaveAsync();
        }

        private async Task<Book> GetOneBookByIdAndChechExits(int id, bool trackChanges)
        {
            //check entity
            var entity = await _manager.Book.GetOneBookByIdAsync(id, trackChanges);
            if (entity is null)
            {
                throw new BookNotFoundException(id);
            }

            return entity;
        }
    }
}
