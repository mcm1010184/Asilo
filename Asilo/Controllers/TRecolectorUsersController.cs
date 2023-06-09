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
    public class TRecolectorUsersController : Controller
    {
        private readonly AsilosAncianosContext _context;
        private readonly IMongoClient _mongoClient;


        public TRecolectorUsersController(AsilosAncianosContext context, IMongoClient mongoClient)
        {
            _context = context;
            _mongoClient = mongoClient;
        }

        // GET: TRecolectorUsers
        public async Task<IActionResult> Index()
        {
              return _context.TRecolectorUser != null ? 
                          View(await _context.TRecolectorUser.ToListAsync()) :
                          Problem("Entity set 'AsilosAncianosContext.TRecolectorUser'  is null.");
        }

        // GET: TRecolectorUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TRecolectorUser == null)
            {
                return NotFound();
            }

            var tRecolectorUser = await _context.TRecolectorUser
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tRecolectorUser == null)
            {
                return NotFound();
            }

            return View(tRecolectorUser);
        }

        // GET: TRecolectorUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TRecolectorUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nonbre,Apellido,SegundoApellido,Ci,Celular,IdUser,Usuario1,Password")] TRecolectorUser tRecolectorUser)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Insertar en SQL Server
                    Usuario u = new()
                    {
                        Usuario1 = tRecolectorUser.Usuario1,
                        Password = tRecolectorUser.Password,
                        Role = "Recolector",
                    };
                    _context.Usuarios.Add(u);
                    await _context.SaveChangesAsync();

                    Recolector r = new()
                    {
                        Id = u.Id,
                        Nonbre = tRecolectorUser.Nonbre,
                        Apellido = tRecolectorUser.Apellido,
                        SegundoApellido = tRecolectorUser.SegundoApellido,
                        Ci = tRecolectorUser.Ci,
                        Celular = tRecolectorUser.Celular,
                     
                    };
                    _context.Recolectors.Add(r);
                    await _context.SaveChangesAsync();

                    // Insertar en MongoDB
                    UsuarioRecolector recolM = new UsuarioRecolector()
                    {
                        IdAux = u.Id,
                        nonbre = tRecolectorUser.Nonbre,
                        apellido = tRecolectorUser.Apellido,
                        segundoApellido = tRecolectorUser.SegundoApellido,
                        ci = tRecolectorUser.Ci,
                        celular = tRecolectorUser.Celular,
                        usuario = tRecolectorUser.Usuario1,
                        contraseña = tRecolectorUser.Password,
                        rol = "RECOLECTOR",
                        estado = 1,
                        fechaRegistro = DateTime.Now,
                        fechaActualizada = null
                    };

                    var database = _mongoClient.GetDatabase("AsilosAncianos");
                    var collection = database.GetCollection<UsuarioRecolector>("usuarios");
                    collection.InsertOne(recolM);

                    transaction.Commit();
                    return RedirectToAction("Index","Recolectors");
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }

            return RedirectToAction("Index", "Home");
        }

        // GET: TRecolectorUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TRecolectorUser == null)
            {
                return NotFound();
            }

            var tRecolectorUser = await _context.TRecolectorUser.FindAsync(id);
            if (tRecolectorUser == null)
            {
                return NotFound();
            }
            return View(tRecolectorUser);
        }

        // POST: TRecolectorUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nonbre,Apellido,SegundoApellido,Ci,Celular,IdUser,Usuario1,Password")] TRecolectorUser tRecolectorUser)
        {
            if (id != tRecolectorUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tRecolectorUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TRecolectorUserExists(tRecolectorUser.Id))
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
            return View(tRecolectorUser);
        }

        // GET: TRecolectorUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TRecolectorUser == null)
            {
                return NotFound();
            }

            var tRecolectorUser = await _context.TRecolectorUser
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tRecolectorUser == null)
            {
                return NotFound();
            }

            return View(tRecolectorUser);
        }

        // POST: TRecolectorUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TRecolectorUser == null)
            {
                return Problem("Entity set 'AsilosAncianosContext.TRecolectorUser'  is null.");
            }
            var tRecolectorUser = await _context.TRecolectorUser.FindAsync(id);
            if (tRecolectorUser != null)
            {
                _context.TRecolectorUser.Remove(tRecolectorUser);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TRecolectorUserExists(int id)
        {
          return (_context.TRecolectorUser?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
