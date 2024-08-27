using bookDemo.Data;
using bookDemo.Modals;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace bookDemo.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {

        /// <summary>
        /// Bütün kitapların listelenmesi
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var books = ApplicationContext.Books;
            return Ok(books);
        }

        /// <summary>
        /// Kitapları ID'ye göre listelenmesi
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public IActionResult GetBook([FromRoute(Name = "id")] int id)
        {
            Book book = ApplicationContext.Books.Where<Book>(b => b.Id.Equals(id)).SingleOrDefault();

            if (book is null)
                return NotFound();


            return Ok(book);
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
                ApplicationContext.Books.Add(book);
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
            //chech book => Girilen Id bilgisi ile kayıtlı kitapların içerisinden eşleme yapılır ve ilgili kitap çekilir.
            var entity = ApplicationContext.Books.Find(b => b.Id.Equals(id));
            if (entity is null)
                return NotFound();  //httpStatus 404;  => Eğer id ile eşleşen kitap bulunamaz ise NotFound:HttpSatus400 döner.

            //chech id  => Girilen id İle girilen book bilgileri içerisinde bulunan book.id bilgisi eşleşme kontrolü yapılır.
            if (id != book.Id)
                return BadRequest();  //httpStatus 400; => Eğer girilen bilgileri uyuşmaz ise BadRequest : HttpStatus400 döner.

            ApplicationContext.Books.Remove(entity);    // -> Koleksiyon içerisinden ilgili kitabın eski halini silerç
            book.Id = entity.Id;    //Yeni veriye eski veriye ait id bilgisi atanır.
            ApplicationContext.Books.Add(book);     // -> Güncellenmiş şekilde tekrar koleksiyona ekleme işlemi gerçekleştirilir.

            return Ok(book);    // Sonuç başarılı olarak döner, body içerisinde güncellenen book verisini döner.

        }

        /// <summary>
        /// Bütün Kitap listesini temizler.
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult DeleteAllBook()
        {
            ApplicationContext.Books.Clear();   //ilgili  liste temizlendi
            return NoContent();
        }

        /// <summary>
        /// Id bilgisi verilen kitabı siler
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        public IActionResult DeleteBook([FromRoute] int id)
        {
            bool result = ApplicationContext.Books.Remove(ApplicationContext.Books.SingleOrDefault(b => b.Id == id)!);  // -> verilen id ile ilişkili book bilsini bulur ve remove ile bunu aynı zamanda siler. Sonuç olarak bool değer döner.

            return result ? NoContent() : NotFound(new { statusCode = 400, message = $"Book with id:{id} could not found!" });   // -> Dönen bool değere göre, true:Başarılı/İçerik yok(NoContent:httpstatus204), false: başarısız/bulunamadı(NotFound:httpstatus400) döner.

        }

        [HttpPatch("{id:int}")]
        public IActionResult PartiallyUpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<Book> bookPatch)
        {
            //Chech => PArametre olarak verilen kitların varlığı kontrol edilir, verilen bilgilere ait kitap bulunuyor mu kontrolü.
            var entity = ApplicationContext.Books.Find(b => b.Id.Equals(id));
            if (entity is null) return NotFound();  //verilen bilgilere ait entity bulunamadığı için 400 döner.

            bookPatch.ApplyTo(entity);  // -> parçalama ve girilen verinin mevcut yapıya aktarılıp parçalı olarak güncelleme işlemi yapılır.
            return NoContent();     //204 döner başarılı işlem.

        }

    }
}
