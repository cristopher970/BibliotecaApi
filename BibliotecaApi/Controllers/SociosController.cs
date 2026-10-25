using Asp.Versioning;
using BibliotecaApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
[Authorize]
public class SociosController : ControllerBase
{
    private readonly BibliotecaContext _context;
    public SociosController(BibliotecaContext context)
    {
        _context = context;
    }

    // GET: api/Socio
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Socio>>> GetSocio()
    {
        return await _context.Socios.ToListAsync();
    }

    // GET: api/Socio/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Socio>> GetSocio(int id)
    {
        var socio = await _context.Socios.FindAsync(id);

        if (socio == null)
        {
            return NotFound();
        }

        return socio;
    }

    // PUT: api/Socio/5
    // Roles: 1 = Administrador, 2 = Bibliotecario
    [Authorize(Roles = "1,2")]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutSocio(int id, Socio socio)
    {
        if (id != socio.Id)
        {
            return BadRequest();
        }

        _context.Entry(socio).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!SocioExists(id))
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

    // POST: api/Socio
    [Authorize(Roles = "1,2")]
    [HttpPost]
    public async Task<ActionResult<Socio>> PostSocio(Socio socio)
    {
        _context.Socios.Add(socio);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetSocio", new { id = socio.Id }, socio);
    }

    // DELETE: api/Socio/5
    // Solo Administrador
    [Authorize(Roles = "1")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSocio(int id)
    {
        var socio = await _context.Socios.FindAsync(id);
        if (socio == null)
        {
            return NotFound();
        }

        _context.Socios.Remove(socio);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool SocioExists(int id)
    {
        return _context.Socios.Any(e => e.Id == id);
    }
}
