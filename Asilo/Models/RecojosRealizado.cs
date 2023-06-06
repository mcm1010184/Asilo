using System;
using System.Collections.Generic;

namespace Asilo.Models;

public partial class RecojosRealizado
{
    public int Id { get; set; }

    public int EstablecimientoId { get; set; }

    public int RecolectorId { get; set; }

    public byte Cantidad { get; set; }

    public DateTime Fecha { get; set; }

    public virtual Establecimiento Establecimiento { get; set; } = null!;

    public virtual Recolector Recolector { get; set; } = null!;
}
