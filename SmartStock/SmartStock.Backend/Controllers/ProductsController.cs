using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartStock.Backend.Data;
using SmartStock.Backend.UnitsOfWork.Interfaces;
using SmartStock.Shared.DTOs;
using SmartStock.Shared.Entites;

namespace SmartStock.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : GenericController<Product>

{
    private readonly IProductsUnitOfWork _productsUnitOfWork;

    public ProductsController(IGenericUnitOfWork<Product> unitOfWork, IProductsUnitOfWork productsUnitOfWork) : base(unitOfWork)
    {
        _productsUnitOfWork = productsUnitOfWork;
    }

    [HttpGet]
    public override async Task<IActionResult> GetAsync()
    {
        var response = await _productsUnitOfWork.GetAsync();
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest();
    }

    [HttpGet("{id}")]
    public override async Task<IActionResult> GetAsync(int id)
    {
        var response = await _productsUnitOfWork.GetAsync(id);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return NotFound(response.Message);
    }

    [HttpGet("combo/{categoryId:int}")]
    public async Task<IActionResult> GetComboAsync(int categoryId)
    {
        return Ok(await _productsUnitOfWork.GetComboAsync(categoryId));
    }

    [HttpPost("full")]
    public async Task<IActionResult> PostAsync(ProductDTO productDTO)
    {
        var action = await _productsUnitOfWork.AddAsync(productDTO);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest(action.Message);
    }

    [HttpPut("full")]
    public async Task<IActionResult> PutAsync(ProductDTO productDTO)
    {
        var action = await _productsUnitOfWork.UpdateAsync(productDTO);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest(action.Message);
    }
}