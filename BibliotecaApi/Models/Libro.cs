using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BibliotecaApi.Models;

public partial class Libro
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El título es obligatorio.")]
    [StringLength(150)]
    public string Titulo { get; set; } = null!;

    [Required(ErrorMessage = "El autor es obligatorio.")]
    [StringLength(100)]
    public string Autor { get; set; } = null!;

    [StringLength(20)]
    public string? Isbn { get; set; }

    [Range(0, 10000, ErrorMessage = "El stock debe estar entre 0 y 10000.")]
    public int Stock { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe indicar una categoría válida.")]
    public int CategoriaId { get; set; }

    public virtual Categorium? Categoria { get; set; } = null;

    [JsonIgnore]
    public virtual ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
}
