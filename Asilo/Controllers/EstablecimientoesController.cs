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

namespace Asilo.Controllers
{
    public class EstablecimientoesController : Controller
    {
        private readonly AsilosAncianosContext _context;
        private readonly IMongoClient _mongoClient;
        private readonly IMongoCollection<UsuarioEstablecimiento> _mongoCollection;

        public EstablecimientoesController(AsilosAncianosContext context, IMongoClient mongoClient)
        {
            _context = context;
            _mongoClient = mongoClient;
            _mongoCollection = _mongoClient.GetDatabase("AsilosAncianos").GetCollection<UsuarioEstablecimiento>("usuarios");
        }
    


        // GET: Establecimientoes
        public async Task<IActionResult> Index()
        {
              return _context.Establecimientos != null ? 
                          View(await _context.Establecimientos.ToListAsync()) :
                          Problem("Entity set 'AsilosAncianosContext.Establecimientos'  is null.");
        }

        // GET: Establecimientoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Establecimientos == null)
            {
                return NotFound();
            }

            var establecimiento = await _context.Establecimientos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (establecimiento == null)
            {
                return NotFound();
            }

            return View(establecimiento);
        }

        // GET: Establecimientoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Establecimientoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Nit,RepresentantePrincipal,Email,Telefono,Celular,Direccion,Latitud,Longitud,TipoEstablecimiento")] Establecimiento establecimiento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(establecimiento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(establecimiento);
        }

        // GET: Establecimientoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var dbEstablecimiento = _context.Establecimientos;
            var LatitudE = (from x in dbEstablecimiento
                             select x.Latitud);
            ViewData["LatitudE"] = LatitudE;
            var LongitudE = (from x in dbEstablecimiento
                            select x.Longitud);
            ViewData["LongitudE"] = LongitudE;

            if (id == null || _context.Establecimientos == null)
            {
                return NotFound();
            }

            var establecimiento = await _context.Establecimientos.FindAsync(id);
            if (establecimiento == null)
            {
                return NotFound();
            }
            return View(establecimiento);
        }

        // POST: Establecimientoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Nit,RepresentantePrincipal,Email,Telefono,Celular,Direccion,Latitud,Longitud,TipoEstablecimiento")] Establecimiento establecimiento)
        {
            if (id != establecimiento.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(establecimiento);
                    await _context.SaveChangesAsync();

                    var filter = Builders<UsuarioEstablecimiento>.Filter.Eq(e => e.IdAux, id);
                    var update = Builders<UsuarioEstablecimiento>.Update
                        .Set(e => e.nombre, establecimiento.Nombre)
                        .Set(e => e.nit, establecimiento.Nit)
                        .Set(e => e.representantePrincipal, establecimiento.RepresentantePrincipal)
                        .Set(e => e.correoElectrónico, establecimiento.Email)
                        .Set(e => e.telefono, establecimiento.Telefono)
                        .Set(e => e.celular, establecimiento.Celular)
                        .Set(e => e.direccion, establecimiento.Direccion)
                        .Set(e => e.latitud, establecimiento.Latitud)
                        .Set(e => e.longitud, establecimiento.Longitud)
                        .Set(e => e.tipoEstablecimiento, establecimiento.TipoEstablecimiento);

                    var updateResult = await _mongoCollection.UpdateOneAsync(filter, update);

                    if (updateResult.ModifiedCount == 0)
                    {
                        // Manejar el caso cuando el documento no existe en MongoDB o no se modificó correctamente
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstablecimientoExists(establecimiento.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(establecimiento);
        }

        // GET: Establecimientoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Establecimientos == null)
            {
                return NotFound();
            }

            var establecimiento = await _context.Establecimientos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (establecimiento == null)
            {
                return NotFound();
            }

            return View(establecimiento);
        }

        // POST: Establecimientoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tPersonPasient = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            try
            {
                tPersonPasient.Estado = 0;
                _context.Update(tPersonPasient);
                await _context.SaveChangesAsync();

                var filter = Builders<UsuarioEstablecimiento>.Filter.Eq(r => r.IdAux, id);
                var update = Builders<UsuarioEstablecimiento>.Update
                    .Set(r => r.estado, 0);


                var updateResult = await _mongoCollection.UpdateOneAsync(filter, update);

            }
            catch (Exception)
            {

            }

            return RedirectToAction("Index", "TEstablecimientoUsers");
        }

        private bool EstablecimientoExists(int id)
        {
          return (_context.Establecimientos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
