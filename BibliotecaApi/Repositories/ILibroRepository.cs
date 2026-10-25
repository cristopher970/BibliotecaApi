using BibliotecaApi.Models;

namespace BibliotecaApi.Repositories
{
    public interface ILibroRepository
    {
        Task<IEnumerable<Libro>> GetAllAsync();
        Task<Libro?> GetByIdAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> CategoriaExistsAsync(int categoriaId);
        Task<bool> IsbnExistsAsync(string isbn, int? excludeId = null);
        Task<bool> TienePrestamosAsync(int id);
        Task AddAsync(Libro libro);
        Task UpdateAsync(Libro libro);
        Task DeleteAsync(Libro libro);
    }
}
