using Asp.Versioning;
using BibliotecaApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
[Authorize]
public class CategoriasController : ControllerBase
{
    private readonly BibliotecaContext _context;
    public CategoriasController(BibliotecaContext context)
    {
        _context = context;
    }

    // GET: api/Categorium
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Categorium>>> GetCategorium()
    {
        return await _context.Categoria.ToListAsync();
    }

    // GET: api/Categorium/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Categorium>> GetCategorium(int id)
    {
        var categorium = await _context.Categoria.FindAsync(id);

        if (categorium == null)
        {
            return NotFound();
        }

        return categorium;
    }

    // PUT: api/Categorium/5
    // Roles: 1 = Administrador, 2 = Bibliotecario
    [Authorize(Roles = "1,2")]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCategorium(int id, Categorium categorium)
    {
        if (id != categorium.Id)
        {
            return BadRequest();
        }

        _context.Entry(categorium).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CategoriumExists(id))
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

    // POST: api/Categorium
    [Authorize(Roles = "1,2")]
    [HttpPost]
    public async Task<ActionResult<Categorium>> PostCategorium(Categorium categorium)
    {
        _context.Categoria.Add(categorium);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetCategorium", new { id = categorium.Id }, categorium);
    }

    // DELETE: api/Categorium/5
    // Solo Administrador
    [Authorize(Roles = "1")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategorium(int id)
    {
        var categorium = await _context.Categoria.FindAsync(id);
        if (categorium == null)
        {
            return NotFound();
        }

        _context.Categoria.Remove(categorium);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CategoriumExists(int id)
    {
        return _context.Categoria.Any(e => e.Id == id);
    }
}