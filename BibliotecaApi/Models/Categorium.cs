using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BibliotecaApi.Models;

public partial class Categorium
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Libro> Libros { get; set; } = new List<Libro>();
}
