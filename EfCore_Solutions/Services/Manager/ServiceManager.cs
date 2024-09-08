using Repositories.Contracts;
using Services.Contracts;

namespace Services.Manager
{
    public class ServiceManager: IServiceManager
    {
        private readonly Lazy<IBookService> _bookService;
        public ServiceManager(IRepositoryManager repositoryManager)
        {
            _bookService = new Lazy<IBookService>(() => new BookManager(repositoryManager));   
        }
        public IBookService BookService  => _bookService.Value;
    }
}
