using Asp.Versioning;
using BibliotecaApi.Models;
using BibliotecaApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
[Authorize]
public class LibrosController : ControllerBase
{
    private readonly ILibroRepository _repo;
    public LibrosController(ILibroRepository repo)
    {
        _repo = repo;
    }

    // GET: api/Libros
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Libro>>> GetLibro()
    {
        return Ok(await _repo.GetAllAsync());
    }

    // GET: api/Libros/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Libro>> GetLibro(int id)
    {
        var libro = await _repo.GetByIdAsync(id);

        if (libro == null)
        {
            return NotFound();
        }

        return libro;
    }

    // PUT: api/Libros/5
    // Roles: 1 = Administrador, 2 = Bibliotecario
    [Authorize(Roles = "1,2")]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutLibro(int id, Libro libro)
    {
        if (id != libro.Id)
        {
            return BadRequest();
        }

        if (!await _repo.CategoriaExistsAsync(libro.CategoriaId))
            return BadRequest(new { mensaje = "La categoría indicada no existe." });

        if (!string.IsNullOrWhiteSpace(libro.Isbn) && await _repo.IsbnExistsAsync(libro.Isbn, id))
            return Conflict(new { mensaje = "Ya existe otro libro con ese ISBN." });

        try
        {
            await _repo.UpdateAsync(libro);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _repo.ExistsAsync(id))
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

    // POST: api/Libros
    [Authorize(Roles = "1,2")]
    [HttpPost]
    public async Task<ActionResult<Libro>> PostLibro(Libro libro)
    {
        if (!await _repo.CategoriaExistsAsync(libro.CategoriaId))
            return BadRequest(new { mensaje = "La categoría indicada no existe." });

        if (!string.IsNullOrWhiteSpace(libro.Isbn) && await _repo.IsbnExistsAsync(libro.Isbn))
            return Conflict(new { mensaje = "Ya existe un libro con ese ISBN." });

        await _repo.AddAsync(libro);

        return CreatedAtAction("GetLibro", new { version = "1", id = libro.Id }, libro);
    }

    // DELETE: api/Libros/5
    // Solo Administrador
    [Authorize(Roles = "1")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLibro(int id)
    {
        var libro = await _repo.GetByIdAsync(id);
        if (libro == null)
        {
            return NotFound();
        }

        if (await _repo.TienePrestamosAsync(id))
            return Conflict(new { mensaje = "No se puede eliminar el libro tiene préstamos asociados." });

        await _repo.DeleteAsync(libro);

        return NoContent();
    }
}