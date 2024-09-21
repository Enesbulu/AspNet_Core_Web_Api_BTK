using AutoMapper;
using Entities.DataTransferObjects;
using Repositories.Contracts;
using Services.Contracts;

namespace Services.Manager
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IBookService> _bookService;
        public ServiceManager(IRepositoryManager repositoryManager, ILoggerService logger, IMapper mapper, IDataShaper<BookDto> shaper)
        {
            _bookService = new Lazy<IBookService>(() => new BookManager(manager: repositoryManager, logger: logger, mapper: mapper,shaper:shaper));
        }
        public IBookService BookService => _bookService.Value;
    }
}
