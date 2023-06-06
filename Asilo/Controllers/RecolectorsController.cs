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
    public class RecolectorsController : Controller
    {
        private readonly AsilosAncianosContext _context;

        public RecolectorsController(AsilosAncianosContext context)
        {
            _context = context;
        }

        // GET: Recolectors
        public async Task<IActionResult> Index()
        {
            var asilosAncianosContext = _context.Recolectors.Include(r => r.IdNavigation);
            return View(await asilosAncianosContext.ToListAsync());
        }

        // GET: Recolectors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Recolectors == null)
            {
                return NotFound();
            }

            var recolector = await _context.Recolectors
                .Include(r => r.IdNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recolector == null)
            {
                return NotFound();
            }

            return View(recolector);
        }

        // GET: Recolectors/Create
        public IActionResult Create()
        {
            ViewData["Id"] = new SelectList(_context.Usuarios, "Id", "Id");
            return View();
        }

        // POST: Recolectors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nonbre,Apellido,SegundoApellido,Ci,Celular")] Recolector recolector)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recolector);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Id"] = new SelectList(_context.Usuarios, "Id", "Id", recolector.Id);
            return View(recolector);
        }

        // GET: Recolectors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Recolectors == null)
            {
                return NotFound();
            }

            var recolector = await _context.Recolectors.FindAsync(id);
            if (recolector == null)
            {
                return NotFound();
            }
            ViewData["Id"] = new SelectList(_context.Usuarios, "Id", "Id", recolector.Id);
            return View(recolector);
        }

        // POST: Recolectors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nonbre,Apellido,SegundoApellido,Ci,Celular")] Recolector recolector)
        {
            if (id != recolector.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recolector);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecolectorExists(recolector.Id))
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
            ViewData["Id"] = new SelectList(_context.Usuarios, "Id", "Id", recolector.Id);
            return View(recolector);
        }

        // GET: Recolectors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Recolectors == null)
            {
                return NotFound();
            }

            var recolector = await _context.Recolectors
                .Include(r => r.IdNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recolector == null)
            {
                return NotFound();
            }

            return View(recolector);
        }

        // POST: Recolectors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Recolectors == null)
            {
                return Problem("Entity set 'AsilosAncianosContext.Recolectors'  is null.");
            }
            var recolector = await _context.Recolectors.FindAsync(id);
            if (recolector != null)
            {
                _context.Recolectors.Remove(recolector);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecolectorExists(int id)
        {
          return (_context.Recolectors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
