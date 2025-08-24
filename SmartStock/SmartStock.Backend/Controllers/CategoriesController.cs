using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartStock.Backend.Data;
using SmartStock.Shared.Entites;

namespace SmartStock.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly DataContext _context;

    public CategoriesController(DataContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get Async all Categories
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        return Ok(await _context.Categories.ToListAsync());
    }

    /// <summary>
    /// Get Async Category by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var product = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    /// <summary>
    /// Post Async Category
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> PostAsync(Category category)
    {
        _context.Add(category);
        await _context.SaveChangesAsync();
        return Ok(category);
    }

    /// <summary>
    ///  Delete Async category by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
        if (category == null)
        {
            return NotFound();
        }

        _context.Remove(category);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// Put Async category
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<IActionResult> PutAsync(Category category)
    {
        _context.Update(category);
        await _context.SaveChangesAsync();
        return Ok(category);
    }
}