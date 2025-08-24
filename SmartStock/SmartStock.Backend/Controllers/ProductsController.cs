using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartStock.Backend.Data;
using SmartStock.Backend.UnitsOfWork;
using SmartStock.Shared.Entites;

namespace SmartStock.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : GenericController<Product>
{
    /// <summary>
    /// Inicializa una nueva instancia de <see cref="ProductsController"/>.
    /// </summary>
    /// <param name="unit">
    /// Implementación de <see cref="IGenericUnitOfWork{Product}"/> que provee las operaciones de acceso a datos.
    /// </param>
    public ProductsController(IGenericUnitOfWork<Product> unit) : base(unit)
    {
    }
}