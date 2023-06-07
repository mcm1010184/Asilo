using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Asilo.Data;
using Asilo.Models;
using System.Security.Claims;

namespace Asilo.Controllers
{
    public class RecojosRealizadoesController : Controller
    {
        private readonly AsilosAncianosContext _context;

        public RecojosRealizadoesController(AsilosAncianosContext context)
        {
            _context = context;
        }

        // GET: RecojosRealizadoes
        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var asilosAncianosContext = _context.RecojosRealizados
                .Include(r => r.Establecimiento)
                .Include(r => r.Recolector)
                .Where(x => x.Fecha == DateTime.Now.Date && x.EstablecimientoId == int.Parse(userId) && x.Estado == 1);
            return View(await asilosAncianosContext.ToListAsync());
        }
        public async Task<IActionResult> List()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var asilosAncianosContext = _context.RecojosRealizados
                .Include(r => r.Establecimiento)
                .Include(r => r.Recolector)
                .Where(x =>x.EstablecimientoId == int.Parse(userId) && x.Estado == 0);
            return View(await asilosAncianosContext.ToListAsync());
        }

        // GET: RecojosRealizadoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RecojosRealizados == null)
            {
                return NotFound();
            }

            var recojosRealizado = await _context.RecojosRealizados
                .Include(r => r.Establecimiento)
                .Include(r => r.Recolector)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recojosRealizado == null)
            {
                return NotFound();
            }

            return View(recojosRealizado);
        }

        // GET: RecojosRealizadoes/Create
        public IActionResult Create()
        {
            ViewData["EstablecimientoId"] = new SelectList(_context.Establecimientos, "Id", "Id");
            ViewData["RecolectorId"] = new SelectList(_context.Recolectors, "Id", "Id");
            return View();
        }

        // POST: RecojosRealizadoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EstablecimientoId,RecolectorId,Cantidad,Fecha")] RecojosRealizado recojosRealizado)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recojosRealizado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EstablecimientoId"] = new SelectList(_context.Establecimientos, "Id", "Id", recojosRealizado.EstablecimientoId);
            ViewData["RecolectorId"] = new SelectList(_context.Recolectors, "Id", "Id", recojosRealizado.RecolectorId);
            return View(recojosRealizado);
        }

        // GET: RecojosRealizadoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RecojosRealizados == null)
            {
                return NotFound();
            }

            var recojosRealizado = await _context.RecojosRealizados.FindAsync(id);
            if (recojosRealizado == null)
            {
                return NotFound();
            }
            ViewData["EstablecimientoId"] = new SelectList(_context.Establecimientos, "Id", "Nombre", recojosRealizado.EstablecimientoId);
            ViewData["RecolectorId"] = new SelectList(_context.Recolectors, "Id", "Nonbre",recojosRealizado.RecolectorId);
            return View(recojosRealizado);
        }

        // POST: RecojosRealizadoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EstablecimientoId,RecolectorId,Cantidad,Fecha")] RecojosRealizado recojosRealizado)
        {
            if (id != recojosRealizado.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    int idR = recojosRealizado.RecolectorId; 
                    recojosRealizado.Estado = 0;
                    _context.Update(recojosRealizado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecojosRealizadoExists(recojosRealizado.Id))
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
            ViewData["EstablecimientoId"] = new SelectList(_context.Establecimientos, "Id", "Id", recojosRealizado.EstablecimientoId);
            ViewData["RecolectorId"] = new SelectList(_context.Recolectors, "Id", "Id", recojosRealizado.RecolectorId);
            return View(recojosRealizado);
        }

        // GET: RecojosRealizadoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RecojosRealizados == null)
            {
                return NotFound();
            }

            var recojosRealizado = await _context.RecojosRealizados
                .Include(r => r.Establecimiento)
                .Include(r => r.Recolector)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recojosRealizado == null)
            {
                return NotFound();
            }

            return View(recojosRealizado);
        }

        // POST: RecojosRealizadoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RecojosRealizados == null)
            {
                return Problem("Entity set 'AsilosAncianosContext.RecojosRealizados'  is null.");
            }
            var recojosRealizado = await _context.RecojosRealizados.FindAsync(id);
            if (recojosRealizado != null)
            {
                _context.RecojosRealizados.Remove(recojosRealizado);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecojosRealizadoExists(int id)
        {
          return (_context.RecojosRealizados?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
