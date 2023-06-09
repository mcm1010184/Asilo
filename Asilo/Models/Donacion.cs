using System;
using System.Collections.Generic;
using System.Drawing;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Asilo.Models;
public class DonacionMongo
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public int? Recolector { get; set; }
    public int? Campana { get; set; }
    public int? Benefactor { get; set; }
    public int? Cantidad { get; set; }
    public string? Description { get; set; }
    public string? TipoDonacion { get; set; }
    public int TipoBeneficiario { get; set; }
    public DateTime FechaDonacion { get; set; }
    public int Recibida { get; set; }
    public int Estado { get; set; }
    public DateTime FechaRegistro { get; set; }
    public DateTime? FechaActualizada { get; set; }
}
public partial class Donacion
{
    public int Id { get; set; }

    public int CampanaId { get; set; }

    public int BenefactorId { get; set; }

    public byte? Cantidad { get; set; }
    public string? Descripcion { get; set; }

    public bool? Recibida { get; set; }

    public byte? TipoBenefactor { get; set; }

    public string TipoDonacion { get; set; } = "Viveres";
    public DateTime Fecha { get; set; }

    public int RecolectorId { get; set; }

    public  Benefactor? Benefactor { get; set; } = null!;

    public  Campana? Campana { get; set; } = null!;

    public  Recolector? Recolector { get; set; } = null!;
}
