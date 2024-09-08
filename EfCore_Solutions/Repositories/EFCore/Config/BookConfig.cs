using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.EFCore.Config
{
    public class BookConfig : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasData(
                new Book { Id = 1, Title = "Karagöz ve Hacıvat", Price = 75 },
                new Book { Id = 2, Title = "Mevlanadan Öğütler", Price = 120 },
                new Book { Id = 3, Title = "Dede Korkut", Price = 50 }
                );
        }
    }
}
