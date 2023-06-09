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
    public class RecolectorsController : Controller
    {

        private readonly AsilosAncianosContext _context;
        private readonly IMongoClient _mongoClient;
        private readonly IMongoCollection<UsuarioRecolector> _mongoCollection;

        public RecolectorsController(AsilosAncianosContext context, IMongoClient mongoClient)
        {
            _context = context;
            _mongoClient = mongoClient;
            _mongoCollection = _mongoClient.GetDatabase("AsilosAncianos").GetCollection<UsuarioRecolector>("usuarios");
        }

        // GET: Recolectors
        public async Task<IActionResult> Index()
        {
            var users = _context.Recolectors.Include(x => x.IdNavigation).Where(x => x.IdNavigation.Estado == 1);
            return View(await users.ToListAsync());
        }
        public async Task<IActionResult> Index1()
        {
            var asilosAncianosContext = _context.Recolectors;
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

                    var filter = Builders<UsuarioRecolector>.Filter.Eq(r => r.IdAux, id);
                    var update = Builders<UsuarioRecolector>.Update
                        .Set(r => r.nonbre, recolector.Nonbre)
                        .Set(r => r.apellido, recolector.Apellido)
                        .Set(r => r.segundoApellido, recolector.SegundoApellido)
                        .Set(r => r.ci, recolector.Ci)
                        .Set(r => r.celular, recolector.Celular);

                    var updateResult = await _mongoCollection.UpdateOneAsync(filter, update);

                    if (updateResult.ModifiedCount == 0)
                    {
                        // Manejar el caso cuando el documento no existe en MongoDB o no se modificó correctamente
                    }
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
            var tPersonPasient = await _context.Usuarios
                  .FirstOrDefaultAsync(m => m.Id == id);
            try
            {
                tPersonPasient.Estado = 0;
                _context.Update(tPersonPasient);
                await _context.SaveChangesAsync();

                var filter = Builders<UsuarioRecolector>.Filter.Eq(r => r.IdAux, id);
                var update = Builders<UsuarioRecolector>.Update
                    .Set(r => r.estado, 0);
            

                var updateResult = await _mongoCollection.UpdateOneAsync(filter, update);

                if (updateResult.ModifiedCount == 0)
                {
                    // Manejar el caso cuando el documento no existe en MongoDB o no se modificó correctamente
                }
            }
            catch (Exception)
            {

            }

            return RedirectToAction("Index");
        }

        private bool RecolectorExists(int id)
        {
          return (_context.Recolectors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
