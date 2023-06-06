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
    public class BenefactorsController : Controller
    {
        private readonly AsilosAncianosContext _context;

        public BenefactorsController(AsilosAncianosContext context)
        {
            _context = context;
        }

        // GET: Benefactors
        public async Task<IActionResult> Index()
        {
            var asilosAncianosContext = _context.Benefactors.Include(b => b.IdNavigation);
            return View(await asilosAncianosContext.ToListAsync());
        }

        // GET: Benefactors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Benefactors == null)
            {
                return NotFound();
            }

            var benefactor = await _context.Benefactors
                .Include(b => b.IdNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (benefactor == null)
            {
                return NotFound();
            }

            return View(benefactor);
        }

        // GET: Benefactors/Create
        public IActionResult Create()
        {
            ViewData["Id"] = new SelectList(_context.Usuarios, "Id", "Id");
            return View();
        }

        // POST: Benefactors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombres,Apellidos,Carnet,Latitud,Longitud,Dirreccion,Email,Telefono,Celular")] Benefactor benefactor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(benefactor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Id"] = new SelectList(_context.Usuarios, "Id", "Id", benefactor.Id);
            return View(benefactor);
        }

        // GET: Benefactors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Benefactors == null)
            {
                return NotFound();
            }

            var benefactor = await _context.Benefactors.FindAsync(id);
            if (benefactor == null)
            {
                return NotFound();
            }
            ViewData["Id"] = new SelectList(_context.Usuarios, "Id", "Id", benefactor.Id);
            return View(benefactor);
        }

        // POST: Benefactors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombres,Apellidos,Carnet,Latitud,Longitud,Dirreccion,Email,Telefono,Celular")] Benefactor benefactor)
        {
            if (id != benefactor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(benefactor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BenefactorExists(benefactor.Id))
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
            ViewData["Id"] = new SelectList(_context.Usuarios, "Id", "Id", benefactor.Id);
            return View(benefactor);
        }

        // GET: Benefactors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Benefactors == null)
            {
                return NotFound();
            }

            var benefactor = await _context.Benefactors
                .Include(b => b.IdNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (benefactor == null)
            {
                return NotFound();
            }

            return View(benefactor);
        }

        // POST: Benefactors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Benefactors == null)
            {
                return Problem("Entity set 'AsilosAncianosContext.Benefactors'  is null.");
            }
            var benefactor = await _context.Benefactors.FindAsync(id);
            if (benefactor != null)
            {
                _context.Benefactors.Remove(benefactor);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BenefactorExists(int id)
        {
          return (_context.Benefactors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
