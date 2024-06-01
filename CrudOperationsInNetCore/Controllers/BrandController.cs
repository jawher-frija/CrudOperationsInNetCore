using CrudOperationsInNetCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudOperationsInNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {

        private readonly BrandDbContext _dbContext;
        public BrandController(BrandDbContext dbContext)
        {
            _dbContext = dbContext;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Brand>>> GetBrands()
        {
            if(_dbContext.Brands == null) 
            {
                return NotFound();
            }
            return await _dbContext.Brands.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Brand>> GetBrand(int id)
        {
            if (_dbContext.Brands == null)
            {
                return NotFound();
            }
           var brand= await _dbContext.Brands.FindAsync(id);
            if(brand == null)
            {
                return NotFound();
            }
            return brand;
        }
        [HttpPost]
        public async Task<ActionResult<Brand>> PostBrand(Brand brand)
        {
            _dbContext.Brands.Add(brand);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBrand), new { id = brand.ID }, brand);
        }

        [HttpPut]
        public async Task<IActionResult> PutBrand(int id, Brand brand)
        {
            if(id!= brand.ID)
            {
                return BadRequest();
            }
            // Indique que l'entité Brand a été modifiée et doit être mise à jour dans la base de données
            _dbContext.Entry(brand).State = EntityState.Modified;
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) 
            {
                if(!BrandAvailable(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }

            }
            return Ok();
        }

        // Vérifie si une entité Brand avec l'identifiant donné existe dans la base de données
        private bool BrandAvailable(int id)
        {
            return(_dbContext.Brands?.Any(x => x.ID == id)).GetValueOrDefault();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            // Vérifie si le jeu de données Brands dans le contexte de la base de données est null
            if (_dbContext.Brands ==null)
            {
                return NotFound();

            }
            // Cherche l'entité Brand avec l'identifiant spécifié de manière asynchrone
            var brand = await _dbContext.Brands.FindAsync(id);
            // Si l'entité Brand n'est pas trouvée, renvoie une réponse NotFound
            if (brand == null)
            {
                return NotFound();
            }
            _dbContext.Brands.Remove(brand);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

            
    }
}
