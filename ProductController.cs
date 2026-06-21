using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.Models;

namespace ProductAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _context.Products.ToListAsync();

            return Ok(products);
        }

        // GET: api/products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound(new
                {
                    Message = "Product not found"
                });

            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            await _context.Products.AddAsync(product);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct),
                new { id = product.Id },
                product);
        }

        // PUT: api/products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product updatedProduct)
        {
            if (id != updatedProduct.Id)
                return BadRequest(new
                {
                    Message = "Id mismatch."
                });

            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound(new
                {
                    Message = "Product not found"
                });

            product.Name = updatedProduct.Name;
            product.Description = updatedProduct.Description;
            product.Price = updatedProduct.Price;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Product updated successfully"
            });
        }
    }
}
