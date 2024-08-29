using bsStoreApp.WebApi.Config;
using bsStoreApp.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace bsStoreApp.WebApi.Repositories
{
    public class RepositoryContext : DbContext
    {
        public DbSet<Book> Books { get; set; }

        public RepositoryContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new BookConfig());  //Config dosyası tanımlaması db işlemleri için tanımlandı. 
        }
    }
}
