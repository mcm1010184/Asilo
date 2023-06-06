using System;
using System.Collections.Generic;

namespace Asilo.Models;

public partial class Donacion
{
    public int Id { get; set; }

    public int CampanaId { get; set; }

    public int BenefactorId { get; set; }

    public bool? Recibida { get; set; }

    public byte? TipoBenefactor { get; set; }

    public string TipoDonacion { get; set; } = null!;

    public int RecolectorId { get; set; }

    public virtual Benefactor Benefactor { get; set; } = null!;

    public virtual Campana Campana { get; set; } = null!;

    public virtual Recolector Recolector { get; set; } = null!;
}
