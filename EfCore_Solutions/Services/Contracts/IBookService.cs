﻿using Entities.DataTransferObjects;
using Entities.Models;

namespace Services.Contracts
{
    public interface IBookService
    {
        IEnumerable<Book> GetAllBooks(bool trackChanges);
        Book GetOneBookById(int id, bool trackChanges);
        Book CreateOneBook(Book book);
        void UpdateOneBook(int id, BookDtoForUpdate bookDto, bool tractChanges);
        void DeleteOneBook(int id, bool trackChanges);
    }
}
