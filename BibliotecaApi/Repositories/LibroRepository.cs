using BibliotecaApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaApi.Repositories
{
    // Todo el acceso a la tabla Libro pasa por esta clase
    public class LibroRepository : ILibroRepository
    {
        private readonly BibliotecaContext _context;

        public LibroRepository(BibliotecaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Libro>> GetAllAsync()
        {
            return await _context.Libros.ToListAsync();
        }

        public async Task<Libro?> GetByIdAsync(int id)
        {
            return await _context.Libros.FindAsync(id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Libros.AnyAsync(e => e.Id == id);
        }

        public async Task<bool> CategoriaExistsAsync(int categoriaId)
        {
            return await _context.Categoria.AnyAsync(c => c.Id == categoriaId);
        }

        public async Task<bool> IsbnExistsAsync(string isbn, int? excludeId = null)
        {
            return await _context.Libros.AnyAsync(l => l.Isbn == isbn && (excludeId == null || l.Id != excludeId));
        }

        public async Task<bool> TienePrestamosAsync(int id)
        {
            return await _context.Prestamos.AnyAsync(p => p.LibroId == id);
        }

        public async Task AddAsync(Libro libro)
        {
            _context.Libros.Add(libro);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Libro libro)
        {
            _context.Entry(libro).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Libro libro)
        {
            _context.Libros.Remove(libro);
            await _context.SaveChangesAsync();
        }
    }
}