using System;
using System.Collections.Generic;

namespace Asilo.Models;

public partial class Imagen
{
    public int Id { get; set; }

    public int CampanaId { get; set; }

    public byte[] Imagen1 { get; set; } = null!;

    public virtual Campana Campana { get; set; } = null!;
}
