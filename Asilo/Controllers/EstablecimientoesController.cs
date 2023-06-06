using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Asilo.Data;
using Asilo.Models;

namespace Asilo.Controllers
{
    public class EstablecimientoesController : Controller
    {
        private readonly AsilosAncianosContext _context;

        public EstablecimientoesController(AsilosAncianosContext context)
        {
            _context = context;
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
            if (_context.Establecimientos == null)
            {
                return Problem("Entity set 'AsilosAncianosContext.Establecimientos'  is null.");
            }
            var establecimiento = await _context.Establecimientos.FindAsync(id);
            if (establecimiento != null)
            {
                _context.Establecimientos.Remove(establecimiento);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EstablecimientoExists(int id)
        {
          return (_context.Establecimientos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
