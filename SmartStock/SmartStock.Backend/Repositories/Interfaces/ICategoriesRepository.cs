using SmartStock.Shared.DTOs;
using SmartStock.Shared.Entites;
using SmartStock.Shared.Responses;

namespace SmartStock.Backend.Repositories.Interfaces;

public interface ICategoriesRepository
{
    /// <summary>
    /// Obtiene una categoría específica según su identificador único.
    /// </summary>
    /// <param name="id">Identificador de la categoría.</param>
    /// <returns>
    /// Retorna un <see cref="ActionResponse{T}"/> con la categoría encontrada,
    /// o un error si no existe.
    /// </returns>
    Task<ActionResponse<Category>> GetAsync(int id);

    /// <summary>
    /// Obtiene todas las categorías registradas en el sistema.
    /// </summary>
    /// <returns>
    /// Retorna un <see cref="ActionResponse{T}"/> con la colección de categorías.
    /// </returns>
    Task<ActionResponse<IEnumerable<Category>>> GetAsync();

    /// <summary>
    /// Obtiene una lista simplificada de categorías para usar en combos o desplegables.
    /// </summary>
    /// <returns>
    /// Retorna una colección enumerable de categorías.
    /// </returns>
    Task<IEnumerable<Category>> GetComboAsync();

    /// <summary>
    ///
    /// </summary>
    /// <param name="pagination"></param>
    /// <returns></returns>
    Task<ActionResponse<IEnumerable<Category>>> GetAsync(PaginationDTO pagination);

    /// <summary>
    ///
    /// </summary>
    /// <param name="pagination"></param>
    /// <returns></returns>
    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
}