using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Asilo.Data;
using Asilo.Models;
using MongoDB.Driver;
using System.Security.Claims;
using Azure;

namespace Asilo.Controllers
{
    public class CampanasController : Controller
    {
        private readonly AsilosAncianosContext _context;
        private readonly IMongoClient _mongoClient;
        private readonly IMongoCollection<Campana> _mongoCollection;

        public CampanasController(AsilosAncianosContext context, IMongoClient mongoClient)
        {
            _context = context;
            _mongoClient = mongoClient;
            _mongoCollection = _mongoClient.GetDatabase("AsilosAncianos").GetCollection<Campana>("campana");
        }

        // GET: Campanas
        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var asilosAncianosContext = _context.Campanas
                 .Where(l => l.Estado == 1 && l.EstablecimientoId == int.Parse(userId))
                .Include(c => c.Establecimiento)
                .Include(c => c.Imagens);
            return View(await asilosAncianosContext.ToListAsync());
        }
        public async Task<IActionResult> IndexCerradas()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var campañasCerradas = await _context.Campanas
               .Where(c => c.FechaCierre < DateTime.Now && c.EstablecimientoId == int.Parse(userId)) // Filtrar campañas cerradas
               .Include(c => c.Donacions)
               .ToListAsync();

            var benefactoresPorCampana = new Dictionary<int, List<string>>();

            foreach (var campana in campañasCerradas)
            {
                var donaciones = await _context.Donacions
                    .Where(d => d.CampanaId == campana.Id)
                    .ToListAsync();

                var benefactoresCampana = new List<string>();

                foreach (var donacion in donaciones)
                {
                    if (donacion.TipoBenefactor == 0)
                    {
                        benefactoresCampana.Add("Donación anónima");
                    }
                    else
                    {
                        if (donacion.TipoBenefactor == 1)
                        {
                            var benefactor = await _context.Benefactors.FindAsync(donacion.BenefactorId);

                            if (benefactor != null)
                            {
                                benefactoresCampana.Add(benefactor.Nombres);
                            }
                        }
                    }
                }

                benefactoresPorCampana.Add(campana.Id, benefactoresCampana);
            }

            ViewData["BenefactoresPorCampana"] = benefactoresPorCampana;

            return View("IndexCerradas", campañasCerradas); // Pasar el contexto de base de datos a la vista utilizando el método View
        }

        // GET: Campanas
        public async Task<IActionResult> Index1()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var asilosAncianosContext = _context.Campanas.Include(c => c.Establecimiento)
                .Where(x => x.FechaCierre >= DateTime.Now.Date);
            return View(await asilosAncianosContext.ToListAsync());
        }

        //GET: Campanas/Inactive
        public async Task<IActionResult> Inactive()
        {
            var asilosAncianosContext = _context.Campanas.Include(c => c.Establecimiento);
            return View(await asilosAncianosContext.ToListAsync());
        }

        // GET: Campanas/Details/5
        public async Task<IActionResult> Details1(int? id)
        {
            if (id == null || _context.Campanas == null)
            {
                return NotFound();
            }

            var campana = await _context.Campanas
                .Include(c => c.Establecimiento)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (campana == null)
            {
                return NotFound();
            }

            return View(campana);
        }
        //GET: Campanas/Details/5 (Inactive)
        public async Task<IActionResult> DetailsInactive(int? id)
        {
            if (id == null || _context.Campanas == null)
            {
                return NotFound();
            }
            var campana = await _context.Campanas
                .Include(c => c.Establecimiento)
                .Include(c => c.Donacions)
                .ThenInclude(d => d.Benefactor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (campana == null)
            {
                return NotFound();
            }
            return View(campana);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Campanas == null)
            {
                return NotFound();
            }
            // Obtener el libro y sus imágenes
            var campana = await _context.Campanas
                .Include(c => c.Imagens)
                .Include(c => c.Establecimiento)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (campana == null)
            {
                return NotFound();
            }

            return View(campana);
        }

        // GET: Campanas/Create
        public IActionResult Create()
        {
            ViewData["EstablecimientoId"] = new SelectList(_context.Establecimientos, "Id", "Id");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Campana campana, List<string> ImagePreviews)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            try
            {
                var campanaa = new Campana();
                campanaa = new Campana
                {
                    Nombre = campana.Nombre,
                    Requerimiento = campana.Requerimiento,
                    FechaInicio = campana.FechaInicio,
                    FechaCierre = campana.FechaCierre,
                    TipoCampaña = campana.TipoCampaña,
                    EstablecimientoId = campana.EstablecimientoId
                };

                _context.Campanas.Add(campanaa);
                _context.SaveChanges();


                // Procesar las imágenes en la vista previa
                foreach (var imagePreview in ImagePreviews)
                {
                    // Convertir la imagen de base64 a un array de bytes
                    var base64Data = imagePreview.Substring(imagePreview.IndexOf(',') + 1);
                    var imageData = Convert.FromBase64String(base64Data);

                    //Agregar la imagen al libro
                    var campanaImage = new Imagen
                    {
                        Imagen1 = imageData,
                        CampanaId = campanaa.Id
                    };
                    _context.Imagens.Add(campanaImage);
                }
                _context.SaveChanges();
                
                
                CampanaMongo campaMo = new CampanaMongo() {
                    IdAux = campanaa.Id,
                    usuarios = int.Parse(userId),
                    nombre = campanaa.Nombre,
                    requerimientos = campanaa.Requerimiento,
                    imágenes = ImagePreviews,
                    fechaInicio = campanaa.FechaInicio,
                    fechaCierra = campanaa.FechaCierre,
                    estado = 1,
                    fechaRegistro = DateTime.Now.Date,
                    fechaActualizada = null,

                };
                var database = _mongoClient.GetDatabase("AsilosAncianos");
                var collection = database.GetCollection<CampanaMongo>("campana");
                collection.InsertOne(campaMo);

      
                return RedirectToAction("Index", "Campanas");
            }
            catch (Exception ex)
            {
                return View("ERROR" + ex);
            }
        }


        // GET: Campanas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Campanas == null)
            {
                return NotFound();
            }
            Imagen ima = new Imagen();
            var campana = _context.Campanas
                .Include(c => c.Imagens)
                .FirstOrDefault(c => c.Id == id);
            DateTime fechaActual = DateTime.Now;
            campana.FechaModificacion = fechaActual;
            if (campana == null)
            {
                return NotFound();
            }
            ViewData["IdImagen"] = new SelectList(_context.Imagens, "Id", "Imagen1", campana.Imagens);
            ViewData["EstablecimientoId"] = new SelectList(_context.Establecimientos, "Id", "Id", campana.EstablecimientoId);
            return View(campana);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Campana campana, List<string> ImagePreviews)
        {
            var campanaEditado = _context.Campanas.Find(id);
            try
            {

                foreach (var imagePreview in ImagePreviews)
                {
                    // Convertir la imagen de base64 a un array de bytes
                    var base64Data = imagePreview.Substring(imagePreview.IndexOf(',') + 1);
                    var imageData = Convert.FromBase64String(base64Data);



                    // Agregar la imagen al libro
                    var campanaImage = new Imagen
                    {
                        Imagen1 = imageData,
                        CampanaId = campana.Id
                    };
                    _context.Imagens.Add(campanaImage);
                }

                campanaEditado.Nombre = campana.Nombre;
                campanaEditado.Requerimiento = campana.Requerimiento;
                campanaEditado.TipoCampaña = campana.TipoCampaña;
                campanaEditado.FechaInicio = campana.FechaInicio;
                campanaEditado.FechaCierre = campana.FechaCierre;
                campanaEditado.EstablecimientoId = campana.EstablecimientoId;

                campanaEditado.FechaModificacion = DateTime.Now;

                _context.Update(campanaEditado);
                await _context.SaveChangesAsync();
                var filter = Builders<Campana>.Filter.Eq(r => r.Id, id);
                var update = Builders<Campana>.Update
                    .Set(r => r.Nombre, campana.Nombre)
                    .Set(r => r.Requerimiento, campana.Requerimiento)
                    .Set(r => r.Imagens, campana.Imagens)
                    .Set(r => r.TipoCampaña, campana.TipoCampaña)
                    .Set(r => r.FechaInicio, campana.FechaInicio)
                    .Set(r => r.FechaCierre, campana.FechaCierre);

                var updateResult = await _mongoCollection.UpdateOneAsync(filter, update);

                if (updateResult.ModifiedCount == 0)
                {
                    // Manejar el caso cuando el documento no existe en MongoDB o no se modificó correctamente
                }


                return RedirectToAction("Index", "Campanas");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampanaExists(campana.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        public ActionResult GetImage(int id)
        {
            var image = _context.Imagens.Find(id);
            if (image == null)
            {
                return NotFound();
            }

            return File(image.Imagen1, "image/jpeg");
        }


        public async Task<IActionResult> Delete(int? id)
        {


            var campana = _context.Campanas
                .Include(c => c.Imagens)
                //.Include(c => c.Establecimiento)
                .FirstOrDefault(c => c.Id == id);
            if (campana == null)
            {
                return NotFound();
            }

            return View(campana);
        }

        // POST: Campanas/Delete/5
        //[HttpPost, ActionName("Delete")]


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var campana = await _context.Campanas
                .Include(c => c.Imagens)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (campana == null)
            {
                return NotFound();
            }


            if (campana.Imagens != null && campana.Imagens.Count > 0)
            {
                _context.Imagens.RemoveRange(campana.Imagens);
            }
            _context.Campanas.Remove(campana);

            await _context.SaveChangesAsync();

            // Eliminar documento de MongoDB
            var filter = Builders<Campana>.Filter.Eq(c => c.Id, id);
            await _mongoCollection.DeleteOneAsync(filter);

            return RedirectToAction(nameof(Index));
        }


        private bool CampanaExists(int id)
        {
            return (_context.Campanas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

