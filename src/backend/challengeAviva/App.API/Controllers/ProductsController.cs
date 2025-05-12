using App.Core.Entities;
using App.Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repo;

        public ProductsController(IProductRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            var products = await _repo.GetByIdsAsync(Array.Empty<Guid>());
            return Ok(products);
        }
    }
}
