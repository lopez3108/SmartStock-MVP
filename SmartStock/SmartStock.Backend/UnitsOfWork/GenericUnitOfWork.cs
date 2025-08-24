using SmartStock.Backend.Repositories;
using SmartStock.Shared.Responses;

namespace SmartStock.Backend.UnitsOfWork;

public class GenericUnitOfWork<T> : IGenericUnitOfWork<T> where T : class
{
    private readonly IGenericRepository<T> _repository;

    public GenericUnitOfWork(IGenericRepository<T> repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Agrega una nueva entidad al repositorio.
    /// </summary>
    /// <param name="model">Entidad a agregar.</param>
    /// <returns>
    /// Un <see cref="ActionResponse{T}"/> que contiene la entidad agregada si la operación fue exitosa,
    /// o información del error en caso contrario.
    /// </returns>
    public virtual async Task<ActionResponse<T>> AddAsync(T model) => await _repository.AddAsync(model);

    /// <summary>
    /// Elimina una entidad del repositorio según su identificador.
    /// </summary>
    /// <param name="id">Identificador de la entidad a eliminar.</param>
    /// <returns>
    /// Un <see cref="ActionResponse{T}"/> con el resultado de la operación.
    /// </returns>
    public virtual async Task<ActionResponse<T>> DeleteAsync(int id) => await _repository.DeleteAsync(id);

    /// <summary>
    /// Obtiene todas las entidades del repositorio.
    /// </summary>
    /// <returns>
    /// Un <see cref="ActionResponse{IEnumerable{T}}"/> con la lista de entidades recuperadas.
    /// </returns>
    public virtual async Task<ActionResponse<IEnumerable<T>>> GetAsync() => await _repository.GetAsync();

    /// <summary>
    /// Obtiene una entidad específica del repositorio según su identificador.
    /// </summary>
    /// <param name="id">Identificador de la entidad a recuperar.</param>
    /// <returns>
    /// Un <see cref="ActionResponse{T}"/> con la entidad encontrada,
    /// o información del error si no se encuentra.
    /// </returns>
    public virtual async Task<ActionResponse<T>> GetAsync(int id) => await _repository.GetAsync(id);

    /// <summary>
    /// Actualiza una entidad existente en el repositorio.
    /// </summary>
    /// <param name="model">Entidad con los valores actualizados.</param>
    /// <returns>
    /// Un <see cref="ActionResponse{T}"/> con la entidad actualizada si la operación fue exitosa,
    /// o información del error en caso contrario.
    /// </returns>
    public virtual async Task<ActionResponse<T>> UpdateAsync(T model) => await _repository.UpdateAsync(model);
}