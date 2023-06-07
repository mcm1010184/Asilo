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
    public class CampanasController : Controller
    {
        private readonly AsilosAncianosContext _context;

        public CampanasController(AsilosAncianosContext context)
        {
            _context = context;
        }

        // GET: Campanas
        public async Task<IActionResult> Index()
        {
            var asilosAncianosContext = _context.Campanas.Include(c => c.Asilo);
            return View(await asilosAncianosContext.ToListAsync());
        }

        // GET: Campanas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Campanas == null)
            {
                return NotFound();
            }

            var campana = await _context.Campanas
                .Include(c => c.Asilo)
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
            ViewData["AsiloId"] = new SelectList(_context.Establecimientos, "Id", "Id");
            return View();
        }

        // POST: Campanas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AsiloId,Nombre,Requerimiento,FechaInicio,FechaCierre,TipoCampaña,Estado,FechaRegistro,FechaModificacion")] Campana campana)
        {
            if (ModelState.IsValid)
            {
                _context.Add(campana);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AsiloId"] = new SelectList(_context.Establecimientos, "Id", "Id", campana.EstablecimientoID);
            return View(campana);
        }

        // GET: Campanas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Campanas == null)
            {
                return NotFound();
            }

            var campana = await _context.Campanas.FindAsync(id);
            if (campana == null)
            {
                return NotFound();
            }
            ViewData["AsiloId"] = new SelectList(_context.Establecimientos, "Id", "Id", campana.EstablecimientoID);
            return View(campana);
        }

        // POST: Campanas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AsiloId,Nombre,Requerimiento,FechaInicio,FechaCierre,TipoCampaña,Estado,FechaRegistro,FechaModificacion")] Campana campana)
        {
            if (id != campana.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(campana);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["AsiloId"] = new SelectList(_context.Establecimientos, "Id", "Id", campana.EstablecimientoID);
            return View(campana);
        }

        // GET: Campanas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Campanas == null)
            {
                return NotFound();
            }

            var campana = await _context.Campanas
                .Include(c => c.Asilo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (campana == null)
            {
                return NotFound();
            }

            return View(campana);
        }

        // POST: Campanas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Campanas == null)
            {
                return Problem("Entity set 'AsilosAncianosContext.Campanas'  is null.");
            }
            var campana = await _context.Campanas.FindAsync(id);
            if (campana != null)
            {
                _context.Campanas.Remove(campana);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CampanaExists(int id)
        {
          return (_context.Campanas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
