using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartStock.Backend.Data;
using SmartStock.Backend.UnitsOfWork.Implementations;
using SmartStock.Backend.UnitsOfWork.Interfaces;
using SmartStock.Shared.Entites;

namespace SmartStock.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : GenericController<Category>
{
    private readonly ICategoriesUnitOfWork _categoriesUnitOfWork;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="CategoriesController"/>.
    /// </summary>
    /// <param name="unit">
    /// Implementación de <see cref="IGenericUnitOfWork{Category}"/> que provee las operaciones de acceso a datos.
    /// </param>
    public CategoriesController(IGenericUnitOfWork<Category> unit, ICategoriesUnitOfWork categoriesUnitOfWork) : base(unit)
    {
        _categoriesUnitOfWork = categoriesUnitOfWork;
    }

    /// <summary>
    /// Obtiene un listado de categorías para ser utilizado en combos o menús desplegables.
    /// </summary>
    /// <returns>Un resultado con la colección de categorías.</returns>
    [HttpGet("combo")]
    public async Task<IActionResult> GetComboAsync()
    {
        return Ok(await _categoriesUnitOfWork.GetComboAsync());
    }

    /// <summary>
    /// Obtiene todas las categorías disponibles.
    /// </summary>
    /// <returns>
    /// Un resultado con la colección de categorías si la operación es exitosa;
    /// de lo contrario, un BadRequest.
    /// </returns>
    [HttpGet]
    public override async Task<IActionResult> GetAsync()
    {
        var response = await _categoriesUnitOfWork.GetAsync();
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest();
    }

    /// <summary>
    /// Obtiene una categoría específica según su identificador.
    /// </summary>
    /// <param name="id">El identificador de la categoría a obtener.</param>
    /// <returns>
    /// Un resultado con la categoría solicitada si la operación es exitosa;
    /// de lo contrario, un NotFound con el mensaje de error.
    /// </returns>
    [HttpGet("{id}")]
    public override async Task<IActionResult> GetAsync(int id)
    {
        var response = await _categoriesUnitOfWork.GetAsync(id);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return NotFound(response.Message);
    }
}