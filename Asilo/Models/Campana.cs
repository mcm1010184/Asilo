using System;
using System.Collections.Generic;

namespace Asilo.Models;

public partial class Campana
{
    public int Id { get; set; }


    public string Nombre { get; set; } = null!;

    public string Requerimiento { get; set; } = null!;

    public DateTime FechaInicio { get; set; }

    public DateTime FechaCierre { get; set; }

    public string? TipoCampaña { get; set; }

    public int? EstablecimientoID { get; set; }
    public byte Estado { get; set; }

    public DateTime FechaRegistro { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual Establecimiento Asilo { get; set; } = null!;

    public virtual ICollection<Donacion> Donacions { get; set; } = new List<Donacion>();

    public virtual ICollection<Imagen> Imagens { get; set; } = new List<Imagen>();
}
