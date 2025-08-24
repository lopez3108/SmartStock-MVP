using SmartStock.Shared.Responses;

namespace SmartStock.Backend.UnitsOfWork;

public interface IGenericUnitOfWork<T> where T : class
{
    /// <summary>
    /// Obtiene una colección de todos los registros del tipo especificado.
    /// </summary>
    /// <returns>Una lista de entidades del tipo <typeparamref name="T"/>.</returns>
    Task<ActionResponse<IEnumerable<T>>> GetAsync();

    /// <summary>
    /// Agrega un nuevo registro a la base de datos.
    /// </summary>
    /// <param name="model">Entidad del tipo <typeparamref name="T"/> a agregar.</param>
    /// <returns>La entidad agregada envuelta en un <see cref="ActionResponse{T}"/>.</returns>
    Task<ActionResponse<T>> AddAsync(T model);

    /// <summary>
    /// Actualiza un registro existente en la base de datos.
    /// </summary>
    /// <param name="model">Entidad del tipo <typeparamref name="T"/> con la información actualizada.</param>
    /// <returns>La entidad actualizada envuelta en un <see cref="ActionResponse{T}"/>.</returns>
    Task<ActionResponse<T>> UpdateAsync(T model);

    /// <summary>
    /// Elimina un registro de la base de datos según su identificador.
    /// </summary>
    /// <param name="id">Identificador único de la entidad a eliminar.</param>
    /// <returns>La entidad eliminada envuelta en un <see cref="ActionResponse{T}"/>.</returns>
    Task<ActionResponse<T>> DeleteAsync(int id);

    /// <summary>
    /// Obtiene un registro específico por su identificador.
    /// </summary>
    /// <param name="id">Identificador único de la entidad.</param>
    /// <returns>La entidad encontrada envuelta en un <see cref="ActionResponse{T}"/>.</returns>
    Task<ActionResponse<T>> GetAsync(int id);
}