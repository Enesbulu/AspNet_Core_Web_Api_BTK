using Entities.DataTransferObjects;
using Entities.Exceptions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BooksController : ControllerBase
    {
        private readonly IServiceManager _manager;

        public BooksController(IServiceManager manager)
        {
            _manager = manager;
        }

        /// <summary>
        /// Bütün kitapları listeler
        /// </summary>
        /// <returns></returns>
        [HttpGet]

        public IActionResult GetAllBooks()
        {

            var books = _manager.BookService.GetAllBooks(false);
            return Ok(books);
        }

        /// <summary>
        /// Kitapları ID'ye göre listelenmesi
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
        {

            var book = _manager.BookService.GetOneBookById(id, false);

            if (book is null)
                throw new BookNotFoundException(id);
            return Ok(book);
        }


        /// <summary>
        /// Yeni bir kitap eklenmesini sağlar.
        /// </summary>
        /// <param name="bookDto"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateOneBook([FromBody] BookDtoForInsetion bookDto)
        {
            if (bookDto is null)
                return BadRequest();    //http: 400;

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState); //http: 422;

            var book = _manager.BookService.CreateOneBook(bookDto);
            //_manager.Save();
            //_context.SaveChanges(); //yapılan değişikliği onaylar ve kaydeder.
            return StatusCode(201, bookDto);    //CreatedAtRoute()
        }


        /// <summary>
        /// Id bilgisine göre verilen kitapı günceller.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bookDto"></param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        public IActionResult UpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] BookDtoForUpdate bookDto)
        {
            if (bookDto is null)
                return BadRequest();    //http: 400;


            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState); //http: 422;

            _manager.BookService.UpdateOneBook(bookDto: bookDto, id: id, tractChanges: false);
            return NoContent();    // Sonuç başarılı olarak döner, body içerisinde veri dönmez. -- 204
        }

        /// <summary>
        /// Bütün Kitap listesini temizler.
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBook([FromRoute(Name = "id")] int id)
        {
            //var entity = _manager.BookService.GetOneBookById(id, false);
            //if (entity is null)
            //{
            //    return NotFound(
            //        new
            //        {
            //            statusCode = 404,
            //            message = $"Book with id:{id} could not found."
            //        }); //404
            //}

            _manager.BookService.DeleteOneBook(id: id, trackChanges: false);
            return NoContent();


        }


        [HttpPatch("{id:int}")]
        public IActionResult PartiallyUpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<BookDtoForUpdate> bookPatch)
        {
            if (bookPatch is null)
                return BadRequest();    //http: 400;


            ////Chech Entity 
            //var bookDto = _manager.BookService.GetOneBookById(id, true);
            var result = _manager.BookService.GetOneBookForPatch(id, false);

            bookPatch.ApplyTo(result.bookDtoForUpdate, ModelState);                 // -> parçalama ve girilen verinin mevcut yapıya aktarılıp parçalı olarak güncelleme işlemi yapılır.

            TryValidateModel(result.bookDtoForUpdate);

            if (!ModelState.IsValid)
                return UnprocessableEntity();   //http: 422;

            //_manager.BookService.UpdateOneBook(id: id, tractChanges: false, bookDto: new BookDtoForUpdate()
            //{
            //    Id = bookDto.Id,
            //    Title = bookDto.Title,
            //    Price = bookDto.Price,
            //});

            _manager.BookService.SaveChangesForPatch(bookDtoForUpdate: result.bookDtoForUpdate, book: result.book);

            return NoContent(); //204 döner başarılı işlem.

        }
    }
}
