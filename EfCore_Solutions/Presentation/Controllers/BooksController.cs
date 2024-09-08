using Entities.Models;
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

        //private readonly IRepositoryManager _manager; //dbrepositori nesnemizden bir örnek oluşturulur.

        //public BooksController( IRepositoryManager manager)

        //{

        //    _manager = manager;
        //    //_context = context; //ctor da çözme işlemi yapıldı.--Dependency Injection
        //}

        /// <summary>
        /// Bütün kitapları listeler
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllBooks()
        {
            try
            {
                var books = _manager.BookService.GetAllBooks(false);
                return Ok(books);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        /// <summary>
        /// Kitapları ID'ye göre listelenmesi
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
        {
            try
            {
                Book book = _manager.BookService.GetOneBookById(id, false);

                if (book is null)
                    return NotFound();
                return Ok(book);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        /// <summary>
        /// Yeni bir kitap eklenmesini sağlar.
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateOneBook([FromBody] Book book)
        {
            try
            {
                if (book is null)
                    return BadRequest();    //http: 400;

                _manager.BookService.CreateOneBook(book);
                //_manager.Save();
                //_context.SaveChanges(); //yapılan değişikliği onaylar ve kaydeder.
                return StatusCode(201, book);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
        }


        /// <summary>
        /// Id bilgisine göre verilen kitapı günceller.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="book"></param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        public IActionResult UpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] Book book)
        {
            try
            {
                if (book is null)
                    return BadRequest();    //400

                _manager.BookService.UpdateOneBook(book: book, id: id, tractChanges: true);
                return NoContent();    // Sonuç başarılı olarak döner, body içerisinde güncellenen book verisini döner. -- 204

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Bütün Kitap listesini temizler.
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBook([FromRoute(Name = "id")] int id)
        {
            try
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
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception(e.Message);
            }
        }


        [HttpPatch("{id:int}")]
        public IActionResult PartiallyUpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<Book> bookPatch)
        {
            try
            {

                //Chech Entity => PArametre olarak verilen kitların varlığı kontrol edilir, verilen bilgilere ait kitap bulunuyor mu kontrolü.
                var entity = _manager.BookService.GetOneBookById(id, true);

                if (entity is null) return NotFound(); //verilen bilgilere ait entity bulunamadığı için 404 döner.

                bookPatch.ApplyTo(entity); // -> parçalama ve girilen verinin mevcut yapıya aktarılıp parçalı olarak güncelleme işlemi yapılır.

                _manager.BookService.UpdateOneBook(book: entity, id: id, tractChanges: false);

                return NoContent(); //204 döner başarılı işlem.
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception(e.Message);
            }
        }
    }
}
