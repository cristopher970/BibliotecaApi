using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BibliotecaApi.Models;

public partial class Rol
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}