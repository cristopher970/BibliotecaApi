using System.ComponentModel.DataAnnotations;

namespace BibliotecaApi.DTOs
{
    public class LibroCreateDto
    {
        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "El título debe tener entre 2 y 150 caracteres.")]
        public string Titulo { get; set; } = null!;

        [Required(ErrorMessage = "El autor es obligatorio.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El autor debe tener entre 2 y 100 caracteres.")]
        public string Autor { get; set; } = null!;

        [StringLength(20, ErrorMessage = "El ISBN no puede superar los 20 caracteres.")]
        public string? Isbn { get; set; }

        [Range(0, 100000, ErrorMessage = "El stock debe estar entre 0 y 100000.")]
        public int Stock { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe indicar una categoría válida.")]
        public int CategoriaId { get; set; }
    }

    // Mismos campos que al crear
    public class LibroUpdateDto : LibroCreateDto
    {
    }

    public class LibroResponseDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string Autor { get; set; } = null!;
        public string? Isbn { get; set; }
        public int Stock { get; set; }
        public int CategoriaId { get; set; }
    }
}
