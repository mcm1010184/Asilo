using System;
using System.Collections.Generic;

namespace Asilo.Models;

public partial class Recolector
{
    public int Id { get; set; }

    public string Nonbre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string? SegundoApellido { get; set; }

    public string Ci { get; set; } = null!;

    public int Celular { get; set; }

    public virtual ICollection<Donacion> Donacions { get; set; } = new List<Donacion>();

    public virtual Usuario IdNavigation { get; set; } = null!;

    public virtual ICollection<RecojosRealizado> RecojosRealizados { get; set; } = new List<RecojosRealizado>();
}
