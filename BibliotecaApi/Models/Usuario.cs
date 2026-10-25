using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BibliotecaApi.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string NombreCompleto { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public int RolId { get; set; }

    [JsonIgnore]
    public virtual ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();

    public virtual Rol? Rol { get; set; } = null;
}
