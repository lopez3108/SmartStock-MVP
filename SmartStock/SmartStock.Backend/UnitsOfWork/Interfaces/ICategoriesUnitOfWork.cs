using SmartStock.Shared.Entites;
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
}