﻿using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;

namespace Repositories.EFCore
{
    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        public BookRepository(RepositoryContext context) : base(context)
        {

        }

        public async Task<PageList<Book>> GetAllBooksAsync(BookParametres bookParametres, bool trackChanges)
        {
            var books = await FindAll(trackChanges)
                .OrderBy(b => b.Id)
                .ToListAsync();

            return PageList<Book>.ToPagedToList(books, bookParametres.PageNumber, bookParametres.PageSize);
        }

        public async Task<Book> GetOneBookByIdAsync(int id, bool trackChanges) => await FindByCondition(b => b.Id.Equals(id), trackChanges).SingleOrDefaultAsync()!;

        public void CreateOneBook(Book book) => Create(book);

        public void UpdateOneBook(Book book) => Update(book);

        public void DeleteOneBook(Book book) => Delete(book);
    }
}
