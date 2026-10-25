using System;
using System.Collections.Generic;

namespace BibliotecaApi.Models;

public partial class Prestamo
{
    public int Id { get; set; }

    public DateTime FechaPrestamo { get; set; }

    public DateTime? FechaDevolucion { get; set; }

    public bool Devuelto { get; set; }

    public int LibroId { get; set; }

    public int SocioId { get; set; }

    public int UsuarioId { get; set; }

    public virtual Libro? Libro { get; set; } = null;

    public virtual Socio? Socio { get; set; } = null;

    public virtual Usuario? Usuario { get; set; } = null;
}
