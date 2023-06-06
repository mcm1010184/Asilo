using System;
using System.Collections.Generic;

namespace Asilo.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Usuario1 { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public byte Estado { get; set; }

    public DateTime FechaRegistro { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual Benefactor? Benefactor { get; set; }

    public virtual Establecimiento IdNavigation { get; set; } = null!;

    public virtual Recolector? Recolector { get; set; }
}
