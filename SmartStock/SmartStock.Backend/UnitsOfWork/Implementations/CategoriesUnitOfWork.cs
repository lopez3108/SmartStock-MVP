using SmartStock.Backend.Repositories.Interfaces;
using SmartStock.Backend.UnitsOfWork.Interfaces;
using SmartStock.Shared.Entites;
using SmartStock.Shared.Responses;

namespace SmartStock.Backend.UnitsOfWork.Implementations;

public class CategoriesUnitOfWork : GenericUnitOfWork<Category>, ICategoriesUnitOfWork
{
    private readonly ICategoriesRepository _categoriesRepository;

    public CategoriesUnitOfWork(IGenericRepository<Category> repository, ICategoriesRepository countriesRepository) : base(repository)
    {
        _categoriesRepository = countriesRepository;
    }

    /// <summary>
    /// Obtiene todas las categorías disponibles.
    /// </summary>
    /// <returns>Un ActionResponse que contiene una colección de categorías y el estado de la operación.</returns>
    public override async Task<ActionResponse<IEnumerable<Category>>> GetAsync()
        => await _categoriesRepository.GetAsync();

    /// <summary>
    /// Obtiene una categoría específica según su identificador.
    /// </summary>
    /// <param name="id">El identificador de la categoría a obtener.</param>
    /// <returns>Un ActionResponse que contiene la categoría solicitada y el estado de la operación.</returns>
    public override async Task<ActionResponse<Category>> GetAsync(int id)
        => await _categoriesRepository.GetAsync(id);

    /// <summary>
    /// Obtiene un listado de categorías para ser utilizado en combos o menús desplegables.
    /// </summary>
    /// <returns>Una colección de categorías.</returns>
    public async Task<IEnumerable<Category>> GetComboAsync()
        => await _categoriesRepository.GetComboAsync();
}