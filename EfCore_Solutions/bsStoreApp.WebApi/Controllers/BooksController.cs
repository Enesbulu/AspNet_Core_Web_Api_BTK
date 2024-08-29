using bsStoreApp.WebApi.Models;
using bsStoreApp.WebApi.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace bsStoreApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly RepositoryContext _context; //dbrepositori nesnemizden bir örnek oluşturulur.

        public BooksController(RepositoryContext context)
        {
            _context = context; //ctor da çözme işlemi yapıldı.--Dependency Injection
        }

        /// <summary>
        /// Bütün kitapları listeler
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllBooks()
        {
            try
            {
                var books = _context.Books.ToList();
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
        public IActionResult GetBook([FromRoute(Name = "id")] int id)
        {
            try
            {
                Book book = _context.Books.Where<Book>(b => b.Id.Equals(id)).SingleOrDefault();

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
                _context.Books.Add(book);
                _context.SaveChanges(); //yapılan değişikliği onaylar ve kaydeder.
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
                //chech book => Girilen Id bilgisi ile kayıtlı kitapların içerisinden eşleme yapılır ve ilgili kitap çekilir.
                var entity = _context.Books.Where(b => b.Id.Equals(id)).SingleOrDefault();
                if (entity is null)
                    return NotFound();  //httpStatus 404;  => Eğer id ile eşleşen kitap bulunamaz ise NotFound:HttpSatus400 döner.

                //chech id  => Girilen id İle girilen book bilgileri içerisinde bulunan book.id bilgisi eşleşme kontrolü yapılır.
                if (id != book.Id)
                    return BadRequest();  //httpStatus 400; => Eğer girilen bilgileri uyuşmaz ise BadRequest : HttpStatus400 döner.

                //EF entity takibi yaptığı için bizim bunu yapmamıza gerek yok. 
                entity.Title = book.Title;  //title ve price eşlemesi yapılması yeterli. İd db tarafından otomatik atanıyor.
                entity.Price = book.Price;  //yani manuel mapping yapılıyor.

                _context.SaveChanges();

                return Ok(book);    // Sonuç başarılı olarak döner, body içerisinde güncellenen book verisini döner.

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
                var entity = _context.Books.Where(b => b.Id.Equals(id)).FirstOrDefault();

                if (entity is null)
                {
                    return NotFound(
                        new
                        {
                            statusCode = 404,
                            message = $"Book with id:{id} could not found."
                        }); //404
                }

                _context.Books.Remove(entity);
                _context.SaveChanges();
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
                var entity = _context.Books.Where(b => b.Id.Equals(id)).FirstOrDefault();

                if (entity is null) return NotFound(); //verilen bilgilere ait entity bulunamadığı için 404 döner.

                bookPatch.ApplyTo(entity); // -> parçalama ve girilen verinin mevcut yapıya aktarılıp parçalı olarak güncelleme işlemi yapılır.

                _context.SaveChanges();

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
