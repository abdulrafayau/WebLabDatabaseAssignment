using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebLabAPI.Data;
using WebLabAPI.Models;

namespace WebLabAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        return await _context.Products.OrderBy(p => p.ProductName).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return NotFound(new { message = "Product not found" });
        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        if (string.IsNullOrWhiteSpace(product.ProductName) ||
            string.IsNullOrWhiteSpace(product.Sku))
            return BadRequest(new { message = "Product name and SKU are required" });

        var existingProduct = await _context.Products
            .FirstOrDefaultAsync(p => p.Sku == product.Sku);
        if (existingProduct != null)
            return BadRequest(new { message = "SKU already exists" });

        product.CreatedAt = DateTime.UtcNow;
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, Product product)
    {
        if (id != product.Id)
            return BadRequest(new { message = "ID mismatch" });

        var existingProduct = await _context.Products.FindAsync(id);
        if (existingProduct == null)
            return NotFound(new { message = "Product not found" });

        existingProduct.ProductName = product.ProductName;
        existingProduct.Sku = product.Sku;
        existingProduct.Description = product.Description;
        existingProduct.Price = product.Price;
        existingProduct.QuantityInStock = product.QuantityInStock;
        existingProduct.Category = product.Category;
        existingProduct.Supplier = product.Supplier;
        existingProduct.UpdatedAt = DateTime.UtcNow;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return BadRequest(new { message = "Update failed" });
        }

        return Ok(existingProduct);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return NotFound(new { message = "Product not found" });

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Product deleted successfully" });
    }
}
