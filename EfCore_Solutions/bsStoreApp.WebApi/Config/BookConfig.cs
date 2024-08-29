using bsStoreApp.WebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bsStoreApp.WebApi.Config
{
    public class BookConfig : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasData(
                new Book{Id = 1,Title = "Karagöz ve Hacıvat",Price = 70},
                new Book{Id = 2,Title = "Şeker Portakalı",Price = 80},
                new Book{Id = 3,Title = "Mesnevi",Price = 60}
                );
        }
    }
}
