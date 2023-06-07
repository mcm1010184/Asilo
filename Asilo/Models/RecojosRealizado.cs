using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Asilo.Models;

public partial class RecojosRealizado
{
    public int Id { get; set; }
    [ScaffoldColumn(false)]
    [Display(Name = "Establecimiento")]
    public int EstablecimientoId { get; set; }
    [ScaffoldColumn(false)]
    [Display(Name = "Recolector")]
    public int RecolectorId { get; set; }

    public byte Cantidad { get; set; }

    public DateTime Fecha { get; set; }
    public byte Estado { get; set; }

    public  Establecimiento? Establecimiento { get; set; } = null!;

    public  Recolector? Recolector { get; set; } = null!;
}
