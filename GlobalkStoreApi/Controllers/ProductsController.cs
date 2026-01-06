using GlobalkStoreApi.Models;
using GlobalkStoreApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GlobalkStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment env;

        public ProductsController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            this.context = context;
            this.env = env;
        }

        [HttpGet]
        public IActionResult GetProducts(int id)
        {
            var products = context.Products.ToList();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = context.Products.Find(id);

            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public IActionResult CreateProduct([FromForm]ProductDto productDto)
        {
            if (productDto.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "The Image File is required");
                return BadRequest(ModelState);
            }

            // save the image on the server
            string imageFileName = DateTime.Now.ToString("yyyyMMddHHssfff");
            imageFileName += Path.GetExtension(productDto.ImageFile.FileName);

            string imagesFolder = env.WebRootPath + "/images/products/";

            using (var stream = System.IO.File.Create(imagesFolder + imageFileName))
            {
                productDto.ImageFile.CopyTo(stream);
            }


            // save product in the database
            Product product = new Product()
            {
                Name = productDto.Name,
                Brand = productDto.Brand,
                Category = productDto.Category,
                Price = productDto.Price,
                Description = productDto.Description ?? "",
                ImageFileName = imageFileName,
                CreatedAt = DateTime.Now
            };

            context.Products.Add(product);
            context.SaveChanges();

            return Ok(product);
        }
    }
}
