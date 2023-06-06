using System;
using System.Collections.Generic;

namespace Asilo.Models;

public partial class Benefactor
{
    public int Id { get; set; }

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string Carnet { get; set; } = null!;

    public byte[] Latitud { get; set; } = null!;

    public byte[] Longitud { get; set; } = null!;

    public string Dirreccion { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int? Telefono { get; set; }

    public int Celular { get; set; }

    public virtual ICollection<Donacion> Donacions { get; set; } = new List<Donacion>();

    public virtual Usuario IdNavigation { get; set; } = null!;
}
