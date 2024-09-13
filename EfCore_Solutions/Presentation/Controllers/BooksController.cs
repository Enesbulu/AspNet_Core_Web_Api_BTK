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
        public async Task<IActionResult> GetAllBooksAsync()
        {

            var books = await _manager.BookService.GetAllBooksAsync(false);
            return Ok(books);
        }

        /// <summary>
        /// Kitapları ID'ye göre listelenmesi
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOneBookAsync([FromRoute(Name = "id")] int id)
        {

            var book = await _manager.BookService.GetOneBooksIdAsync(id, false);

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
        public async Task<IActionResult> CreateOneBookAsync([FromBody] BookDtoForInsetion bookDto)
        {
            if (bookDto is null)
                return BadRequest();    //http: 400;

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState); //http: 422;

            var book = await _manager.BookService.CreateOneBookAsync(bookDto);

            return StatusCode(201, bookDto);    //CreatedAtRoute()
        }


        /// <summary>
        /// Id bilgisine göre verilen kitapı günceller.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bookDto"></param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateOneBookAsync([FromRoute(Name = "id")] int id, [FromBody] BookDtoForUpdate bookDto)
        {
            if (bookDto is null)
                return BadRequest();    //http: 400;


            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState); //http: 422;

            await _manager.BookService.UpdateOneBookAsync(bookDto: bookDto, id: id, tractChanges: false);
            return NoContent();    // Sonuç başarılı olarak döner, body içerisinde veri dönmez. -- 204
        }

        /// <summary>
        /// Bütün Kitap listesini temizler.
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOneBook([FromRoute(Name = "id")] int id)
        {
           await _manager.BookService.DeleteOneBookAsync(id: id, trackChanges: false);
            return NoContent();
        }


        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PartiallyUpdateOneBookAsync([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<BookDtoForUpdate> bookPatch)
        {
            if (bookPatch is null)
                return BadRequest();    //http: 400;

            var result =await _manager.BookService.GetOneBookForPatchAsync(id, false);

            bookPatch.ApplyTo(result.bookDtoForUpdate, ModelState);                 // -> parçalama ve girilen verinin mevcut yapıya aktarılıp parçalı olarak güncelleme işlemi yapılır.

            TryValidateModel(result.bookDtoForUpdate);

            if (!ModelState.IsValid)
                return UnprocessableEntity();   //http: 422;

            await _manager.BookService.SaveChangesForPatchAsync(bookDtoForUpdate: result.bookDtoForUpdate, book: result.book);

            return NoContent(); //204 döner başarılı işlem.

        }
    }
}
