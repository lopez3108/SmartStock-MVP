using SmartStock.Shared.DTOs;
using SmartStock.Shared.Entities;
using SmartStock.Shared.Responses;

namespace SmartStock.Backend.UnitsOfWork.Interfaces;

public interface ICategoriesUnitOfWork
{
    /// <summary>
    /// Obtiene una categoría específica según su identificador.
    /// </summary>
    /// <param name="id">El identificador de la categoría a obtener.</param>
    /// <returns>Un ActionResponse que contiene la categoría solicitada y el estado de la operación.</returns>
    Task<ActionResponse<Category>> GetAsync(int id);

    /// <summary>
    /// Obtiene todas las categorías disponibles.
    /// </summary>
    /// <returns>Un ActionResponse que contiene una colección de categorías y el estado de la operación.</returns>
    Task<ActionResponse<IEnumerable<Category>>> GetAsync();

    /// <summary>
    /// Obtiene un listado de categorías para ser utilizado en combos o menús desplegables.
    /// </summary>
    /// <returns>Una colección de categorías.</returns>
    Task<IEnumerable<Category>> GetComboAsync();

    /// <summary>
    /// Obtiene de forma paginada el listado de categorías según los parámetros de paginación recibidos.
    /// </summary>
    /// <param name="pagination"></param>
    /// <returns></returns>
    Task<ActionResponse<IEnumerable<Category>>> GetAsync(PaginationDTO pagination);

    /// <summary>
    /// Obtiene el número total de registros de categorías aplicando los parámetros de paginación especificados.
    /// </summary>
    /// <param name="pagination"></param>
    /// <returns></returns>
    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}