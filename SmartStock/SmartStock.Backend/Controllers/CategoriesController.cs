using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartStock.Backend.Data;
using SmartStock.Backend.UnitsOfWork;
using SmartStock.Shared.Entites;

namespace SmartStock.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : GenericController<Category>
{
    /// <summary>
    /// Inicializa una nueva instancia de <see cref="CategoriesController"/>.
    /// </summary>
    /// <param name="unit">
    /// Implementación de <see cref="IGenericUnitOfWork{Category}"/> que provee las operaciones de acceso a datos.
    /// </param>
    public CategoriesController(IGenericUnitOfWork<Category> unit) : base(unit)
    {
    }
}