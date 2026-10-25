using Asp.Versioning;
using BibliotecaApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
[Authorize]
public class PrestamosController : ControllerBase
{
    private readonly BibliotecaContext _context;
    public PrestamosController(BibliotecaContext context)
    {
        _context = context;
    }

    // GET: api/Prestamo
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Prestamo>>> GetPrestamo()
    {
        return await _context.Prestamos.ToListAsync();
    }

    // GET: api/Prestamo/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Prestamo>> GetPrestamo(int id)
    {
        var prestamo = await _context.Prestamos.FindAsync(id);

        if (prestamo == null)
        {
            return NotFound();
        }

        return prestamo;
    }

    // PUT: api/Prestamo/5
    // Roles: 1 = Administrador, 2 = Bibliotecario
    [Authorize(Roles = "1,2")]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPrestamo(int id, Prestamo prestamo)
    {
        if (id != prestamo.Id)
        {
            return BadRequest();
        }

        _context.Entry(prestamo).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PrestamoExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Prestamo
    [Authorize(Roles = "1,2")]
    [HttpPost]
    public async Task<ActionResult<Prestamo>> PostPrestamo(Prestamo prestamo)
    {
        _context.Prestamos.Add(prestamo);
            await _context.SaveChangesAsync();

        return CreatedAtAction("GetPrestamo", new { id = prestamo.Id }, prestamo);
    }

    // DELETE: api/Prestamo/5
    // Solo Administrador
    [Authorize(Roles = "1")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePrestamo(int id)
    {
        var prestamo = await _context.Prestamos.FindAsync(id);
        if (prestamo == null)
        {
            return NotFound();
        }

        _context.Prestamos.Remove(prestamo);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool PrestamoExists(int id)
    {
        return _context.Prestamos.Any(e => e.Id == id);
    }
}
