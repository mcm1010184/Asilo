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
    public class DonacionsController : Controller
    {
        private readonly AsilosAncianosContext _context;

        public DonacionsController(AsilosAncianosContext context)
        {
            _context = context;
        }

        // GET: Donacions
        public async Task<IActionResult> Index()
        {
            var asilosAncianosContext = _context.Donacions.Include(d => d.Benefactor).Include(d => d.Campana).Include(d => d.Recolector);
            return View(await asilosAncianosContext.ToListAsync());
        }
        public async Task<IActionResult> List()
        {
            var asilosAncianosContext = _context.Donacions.Include(d => d.Benefactor).Include(d => d.Campana);
            return View(await asilosAncianosContext.ToListAsync());
        }

        // GET: Donacions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Donacions == null)
            {

                return NotFound();
            }

            var donacion = await _context.Donacions
                .Include(d => d.Benefactor)
                .Include(d => d.Campana)
                .Include(d => d.Recolector)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (donacion == null)
            {
                return NotFound();
            }

            return View(donacion);
        }

        // GET: Donacions/Create
        public IActionResult Create()
        {
            ViewData["BenefactorId"] = new SelectList(_context.Benefactors, "Id", "Id");
            ViewData["CampanaId"] = new SelectList(_context.Campanas, "Id", "Id");
            ViewData["RecolectorId"] = new SelectList(_context.Recolectors, "Id", "Id");
            return View();
        }

        // POST: Donacions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CampanaId,BenefactorId,Cantidad,Descripcion,Recibida,TipoBenefactor,TipoDonacion,Fecha,RecolectorId")] Donacion donacion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(donacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BenefactorId"] = new SelectList(_context.Benefactors, "Id", "Id", donacion.BenefactorId);
            ViewData["CampanaId"] = new SelectList(_context.Campanas, "Id", "Id", donacion.CampanaId);
            ViewData["RecolectorId"] = new SelectList(_context.Recolectors, "Id", "Id", donacion.RecolectorId);
            return View(donacion);
        }

        // GET: Donacions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Donacions == null)
            {
                return NotFound();
            }

            var donacion = await _context.Donacions.FindAsync(id);
            if (donacion == null)
            {
                return NotFound();
            }
            ViewData["BenefactorId"] = new SelectList(_context.Benefactors, "Id", "Id", donacion.BenefactorId);
            ViewData["CampanaId"] = new SelectList(_context.Campanas, "Id", "Id", donacion.CampanaId);
            ViewData["RecolectorId"] = new SelectList(_context.Recolectors, "Id", "Id", donacion.RecolectorId);
            return View(donacion);
        }

        // POST: Donacions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CampanaId,BenefactorId,Recibida,TipoBenefactor,TipoDonacion,Fecha,RecolectorId")] Donacion donacion)
        {
            if (id != donacion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(donacion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonacionExists(donacion.Id))
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
            ViewData["BenefactorId"] = new SelectList(_context.Benefactors, "Id", "Id", donacion.BenefactorId);
            ViewData["CampanaId"] = new SelectList(_context.Campanas, "Id", "Id", donacion.CampanaId);
            ViewData["RecolectorId"] = new SelectList(_context.Recolectors, "Id", "Id", donacion.RecolectorId);
            return View(donacion);
        }

        // GET: Donacions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Donacions == null)
            {
                return NotFound();
            }

            var donacion = await _context.Donacions
                .Include(d => d.Benefactor)
                .Include(d => d.Campana)
                .Include(d => d.Recolector)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (donacion == null)
            {
                return NotFound();
            }

            return View(donacion);
        }

        // POST: Donacions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Donacions == null)
            {
                return Problem("Entity set 'AsilosAncianosContext.Donacions'  is null.");
            }
            var donacion = await _context.Donacions.FindAsync(id);
            if (donacion != null)
            {
                _context.Donacions.Remove(donacion);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DonacionExists(int id)
        {
          return (_context.Donacions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
