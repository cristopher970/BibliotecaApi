using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BibliotecaApi.Models;

public partial class Socio
{
    public int Id { get; set; }

    public string Rut { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string? Telefono { get; set; }

    [JsonIgnore]
    public virtual ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
}
