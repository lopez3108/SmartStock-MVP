using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartStock.Backend.Data;
using SmartStock.Shared.Entites;

namespace SmartStock.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly DataContext _context;

    public ProductsController(DataContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get Async all Products
    /// </summary>
    /// <returns></returns>
    ///
    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        return Ok(await _context.Products.ToListAsync());
    }

    /// <summary>
    /// Get Async Product by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    ///
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(c => c.ProductId == id);
        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    /// <summary>
    /// Post Async Product
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    ///
    [HttpPost]
    public async Task<IActionResult> PostAsync(Product product)
    {
        _context.Add(product);
        await _context.SaveChangesAsync();
        return Ok(product);
    }

    /// <summary>
    /// Delete Async Prodcut by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    ///
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var country = await _context.Products.FirstOrDefaultAsync(c => c.ProductId == id);
        if (country == null)
        {
            return NotFound();
        }

        _context.Remove(country);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// Put Async product
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    ///
    [HttpPut]
    public async Task<IActionResult> PutAsync(Product product)
    {
        _context.Update(product);
        await _context.SaveChangesAsync();
        return Ok(product);
    }
}