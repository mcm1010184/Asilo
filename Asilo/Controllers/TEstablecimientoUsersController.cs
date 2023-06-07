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
    public class TEstablecimientoUsersController : Controller
    {
        private readonly AsilosAncianosContext _context;

        public TEstablecimientoUsersController(AsilosAncianosContext context)
        {
            _context = context;
        }

        // GET: TEstablecimientoUsers
        public async Task<IActionResult> Index()
        {
            var users = _context.Establecimientos.Include(x => x.Usuario).Where(x => x.Usuario.Estado == 1);
            return View(await users.ToListAsync());
        }

        // GET: TEstablecimientoUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TEstablecimientoUser == null)
            {
                return NotFound();
            }

            var tEstablecimientoUser = await _context.TEstablecimientoUser
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tEstablecimientoUser == null)
            {
                return NotFound();
            }

            return View(tEstablecimientoUser);
        }

        // GET: TEstablecimientoUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TEstablecimientoUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Nit,RepresentantePrincipal,Email,Telefono,Celular,Direccion,Latitud,Longitud,TipoEstablecimiento,Id,Usuario1,Password,Role")] TEstablecimientoUser tEstablecimientoUser)
        {
            if (ModelState.IsValid)
            {

            }
            using (var context = _context.Database.BeginTransaction())
            {
                try
                {
                    Usuario u = new()
                    {
                       
                        Usuario1 = tEstablecimientoUser.Usuario1,
                        Password = tEstablecimientoUser.Password,
                        Role = "Establecimiento",
                    };
                    _context.Usuarios.Add(u);
                    await _context.SaveChangesAsync();

                    Establecimiento e = new()
                    {
                        Id = u.Id,
                        Nombre = tEstablecimientoUser.Nombre,
                        Nit = tEstablecimientoUser.Nit,
                        RepresentantePrincipal = tEstablecimientoUser.RepresentantePrincipal,
                        Email = tEstablecimientoUser.Email,
                        Telefono = tEstablecimientoUser.Telefono,
                        Celular = tEstablecimientoUser.Celular,
                        Direccion = tEstablecimientoUser.Direccion,
                        Latitud = tEstablecimientoUser.Latitud,
                        Longitud = tEstablecimientoUser.Longitud,
                        TipoEstablecimiento = tEstablecimientoUser.TipoEstablecimiento
                    };
                    _context.Establecimientos.Add(e);
                    await _context.SaveChangesAsync();
                    context.Commit();
                }
                catch (Exception)
                {
                   
                    context.Rollback();
                }
            }
            return RedirectToAction("Index", "Home");
          
        }

        // GET: TEstablecimientoUsers/Edit/5
       public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Establecimientos == null)
            {
                return NotFound();
            }

            var tEstablecimientoUser = await _context.Establecimientos.FindAsync(id);
            if (tEstablecimientoUser == null)
            {
                return NotFound();
            }
            return View(tEstablecimientoUser);
        }

        // POST: TEstablecimientoUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Nit,RepresentantePrincipal,Email,Telefono,Celular,Direccion,Latitud,Longitud,TipoEstablecimiento,IdUser")] TEstablecimientoUser tEstablecimientoUser)
        {
            if (id != tEstablecimientoUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tEstablecimientoUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TEstablecimientoUserExists(tEstablecimientoUser.Id))
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
            return View(tEstablecimientoUser);
        }

        // GET: TEstablecimientoUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TEstablecimientoUser == null)
            {
                return NotFound();
            }

            var tEstablecimientoUser = await _context.TEstablecimientoUser
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tEstablecimientoUser == null)
            {
                return NotFound();
            }

            return View(tEstablecimientoUser);
        }

        // POST: TEstablecimientoUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TEstablecimientoUser == null)
            {
                return Problem("Entity set 'AsilosAncianosContext.TEstablecimientoUser'  is null.");
            }
            var tEstablecimientoUser = await _context.TEstablecimientoUser.FindAsync(id);
            if (tEstablecimientoUser != null)
            {
                _context.TEstablecimientoUser.Remove(tEstablecimientoUser);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TEstablecimientoUserExists(int id)
        {
          return (_context.TEstablecimientoUser?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
