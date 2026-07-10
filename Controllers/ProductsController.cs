using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFirstWebAPI.Filters;
using MyFirstWebAPI.Models;

namespace MyFirstWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]

    public class ProductsController : ControllerBase
    {
        private readonly ProductsContext _dbContext;

        public ProductsController(ProductsContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        [Route("")]
        public ActionResult<int> CreateProduct(Product product)
        {
            // ActionResult<int> indicates that the method returns an HTTP response with
            // a status code and an integer value (the ID of the created product).
            _dbContext.Set<Product>().Add(product);
            _dbContext.SaveChanges();
            return Ok(product.Id);
        }

        [HttpPut]
        [Route("")]
        public ActionResult UpdateProduct(Product product)
        {
            var existingProduct = _dbContext.Set<Product>().Find(product.Id);
            if (existingProduct == null)
            {
                return NotFound($"Product with ID {product.Id} not found.");
            }
            existingProduct.Name = product.Name;
            existingProduct.Sku = product.Sku;
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<Product>> GetAllProducts()
        {
            var products = _dbContext.Set<Product>().ToList();
            if (products.Count == 0)
            {
                return NotFound("No products found in the database.");
            }
            return Ok(products);
        }

        [HttpGet]
        [Route("{id}")]
        [CheckPermission(Permission.ReadProduct)]
        public ActionResult<Product> GetProductById(int id)
        {
            var product = _dbContext.Set<Product>().Find(id);
            if (product == null)
            {
                return NotFound($"Product with ID {id} not found.");
            }
            return Ok(product);
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult DeleteProduct(int id)
        {
            var existingProduct = _dbContext.Set<Product>().Find(id);
            if (existingProduct == null)
            {
                return NotFound($"Product with ID {id} not found.");
            }
            _dbContext.Set<Product>().Remove(existingProduct);
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}
