using System;
using System.Collections.Generic;
using System.Drawing;

namespace Asilo.Models;

public partial class Donacion
{
    public int Id { get; set; }

    public int CampanaId { get; set; }

    public int BenefactorId { get; set; }

    public byte? Cantidad { get; set; }
    public string? Descripcion { get; set; }

    public bool? Recibida { get; set; }

    public byte? TipoBenefactor { get; set; }

    public string TipoDonacion { get; set; } = null!;
    public DateTime Fecha { get; set; }

    public int RecolectorId { get; set; }

    public  Benefactor? Benefactor { get; set; } = null!;

    public  Campana? Campana { get; set; } = null!;

    public  Recolector? Recolector { get; set; } = null!;
}
