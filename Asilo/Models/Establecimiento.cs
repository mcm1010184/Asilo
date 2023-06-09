﻿using System;
using System.Collections.Generic;

namespace Asilo.Models;

public partial class Establecimiento
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Nit { get; set; } = null!;

    public string RepresentantePrincipal { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int? Telefono { get; set; }

    public int Celular { get; set; }

    public string? Direccion { get; set; }

    public byte[] Latitud { get; set; } = null!;

    public byte[] Longitud { get; set; } = null!;

    public string TipoEstablecimiento { get; set; } = null!;

    public virtual ICollection<Campana> Campanas { get; set; } = new List<Campana>();

    public virtual ICollection<RecojosRealizado> RecojosRealizados { get; set; } = new List<RecojosRealizado>();

    public virtual Usuario? Usuario { get; set; }
}
