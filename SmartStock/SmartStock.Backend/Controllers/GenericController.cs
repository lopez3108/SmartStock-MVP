using Microsoft.AspNetCore.Mvc;
using SmartStock.Backend.UnitsOfWork.Interfaces;

namespace SmartStock.Backend.Controllers;

/// <summary>
/// Controlador genérico que proporciona endpoints básicos (CRUD)
/// para cualquier entidad a través de un IGenericUnitOfWork.
/// </summary>
/// <typeparam name="T">Tipo de entidad gestionada por el controlador.</typeparam>
public class GenericController<T> : Controller where T : class
{
    private readonly IGenericUnitOfWork<T> _unitOfWork;

    /// <summary>
    /// Inicializa una nueva instancia del <see cref="GenericController{T}"/> con el UnitOfWork inyectado.
    /// </summary>
    /// <param name="unitOfWork">Unidad de trabajo genérica para la entidad T.</param>
    public GenericController(IGenericUnitOfWork<T> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Obtiene todos los registros de la entidad T.
    /// </summary>
    /// <returns>Un listado de entidades si la operación fue exitosa; de lo contrario, un error.</returns>
    [HttpGet]
    public virtual async Task<IActionResult> GetAsync()
    {
        var action = await _unitOfWork.GetAsync();
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest();
    }

    /// <summary>
    /// Obtiene un registro específico de la entidad T por su identificador.
    /// </summary>
    /// <param name="id">Identificador único de la entidad.</param>
    /// <returns>La entidad solicitada si existe; de lo contrario, NotFound.</returns>
    [HttpGet("{id}")]
    public virtual async Task<IActionResult> GetAsync(int id)
    {
        var action = await _unitOfWork.GetAsync(id);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return NotFound();
    }

    /// <summary>
    /// Crea un nuevo registro de la entidad T.
    /// </summary>
    /// <param name="model">Entidad a crear.</param>
    /// <returns>La entidad creada si la operación fue exitosa; de lo contrario, un error.</returns>
    [HttpPost]
    public virtual async Task<IActionResult> PostAsync(T model)
    {
        var action = await _unitOfWork.AddAsync(model);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest(action.Message);
    }

    /// <summary>
    /// Actualiza un registro existente de la entidad T.
    /// </summary>
    /// <param name="model">Entidad con los datos actualizados.</param>
    /// <returns>La entidad actualizada si la operación fue exitosa; de lo contrario, un error.</returns>
    [HttpPut]
    public virtual async Task<IActionResult> PutAsync(T model)
    {
        var action = await _unitOfWork.UpdateAsync(model);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest(action.Message);
    }

    /// <summary>
    /// Elimina un registro de la entidad T por su identificador.
    /// </summary>
    /// <param name="id">Identificador único de la entidad.</param>
    /// <returns>NoContent si la eliminación fue exitosa; de lo contrario, un error.</returns>
    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> DeleteAsync(int id)
    {
        var action = await _unitOfWork.DeleteAsync(id);
        if (action.WasSuccess)
        {
            return NoContent();
        }
        return BadRequest(action.Message);
    }
}