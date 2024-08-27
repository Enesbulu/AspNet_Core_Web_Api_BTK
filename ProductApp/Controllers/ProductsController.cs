using Microsoft.AspNetCore.Mvc;
using ProductApp.Models;

namespace ProductApp.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;

        //Dependency Injection işlemi.
        public ProductsController(ILogger<ProductsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAllProduct()
        {
            var products = new List<Product>()
            {
                new Product() {Id = 1,ProductName = "Computer"},
                new Product() {Id = 2,ProductName = "Laptop"},
                new Product() {Id = 3,ProductName = "Mouse"},
            };

            _logger.LogInformation("GellAllProducts action hasbenn called");    //Console da action metod işlemi ile birlikte info message bastırma işlemi yapıldı.
            return Ok(products);
        }


        [HttpPost]
        public IActionResult GetAllProduct2([FromBody] Product product)
        {

           _logger.LogWarning("Product has been created.");
            return StatusCode(201);
        }
    }
}
