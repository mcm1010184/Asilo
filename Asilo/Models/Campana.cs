using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Asilo.Models;
public class CampanaMongo
{
    [BsonId]
    public int? IdAux { get; set; }
    public int? usuarios { get; set; }
    public string? nombre { get; set; }
    public string? requerimientos { get; set; }
    public List<string>? imágenes { get; set; }
    public DateTime fechaInicio { get; set; }
    public DateTime fechaCierra { get; set; }
    public int estado { get; set; }
    public DateTime fechaRegistro { get; set; }
    public DateTime? fechaActualizada { get; set; }
}
public partial class Campana
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Requerimiento { get; set; } = null!;

    public DateTime FechaInicio { get; set; }

    public DateTime FechaCierre { get; set; }

    public string? TipoCampaña { get; set; }

    public int EstablecimientoId { get; set; }

    public byte Estado { get; set; }

    public DateTime FechaRegistro { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual ICollection<Donacion> Donacions { get; set; } = new List<Donacion>();

    public virtual Establecimiento Establecimiento { get; set; } = null!;

    public virtual ICollection<Imagen> Imagens { get; set; } = new List<Imagen>();
}
