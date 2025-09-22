using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TenantService.Data;
using TenantService.Models;

namespace TenantService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TenantsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TenantsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tenants = await _context.Tenants.AsNoTracking().ToListAsync();
            return Ok(tenants);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var tenant = await _context.Tenants
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);
            
            if (tenant == null)
            {
                return NotFound();
            }
            return Ok(tenant);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Tenant tenant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            tenant.Id = Guid.NewGuid();
            tenant.CreatedAt = DateTime.UtcNow;
            
            _context.Tenants.Add(tenant);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetById), new { id = tenant.Id }, tenant);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Tenant updatedTenant)
        {
            if (id != updatedTenant.Id)
            {
                return BadRequest("ID in the URL does not match the ID in the request body.");
            }

            var existingTenant = await _context.Tenants.FindAsync(id);
            if (existingTenant == null)
            {
                return NotFound();
            }

            existingTenant.Name = updatedTenant.Name;
            existingTenant.Domain = updatedTenant.Domain;
            existingTenant.ConnectionString = updatedTenant.ConnectionString;
            existingTenant.IsActive = updatedTenant.IsActive;
            existingTenant.ModifiedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await TenantExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }
        
        private async Task<bool> TenantExists(Guid id)
        {
            return await _context.Tenants.AnyAsync(e => e.Id == id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var tenant = await _context.Tenants.FindAsync(id);
            if (tenant == null)
            {
                return NotFound();
            }

            _context.Tenants.Remove(tenant);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
    }
}
