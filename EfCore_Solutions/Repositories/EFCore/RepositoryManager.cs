using Repositories.Contracts;

namespace Repositories.EFCore
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _context;
        private readonly Lazy<IBookRepository> _bookRepository;

        public RepositoryManager(RepositoryContext context)
        {
            _context = context;
            _bookRepository = new Lazy<IBookRepository>(()=> new BookRepository(context));
        }

        public IBookRepository Book => _bookRepository.Value;
        public async Task SaveAsync() => await _context.SaveChangesAsync();
    }
}
