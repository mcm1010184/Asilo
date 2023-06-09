using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Asilo.Data;
using Asilo.Models;
using ZstdSharp.Unsafe;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Asilo.Controllers
{
    public class TEstablecimientoUsersController : Controller
    {
        private readonly AsilosAncianosContext _context;
        private readonly IMongoClient _mongoClient;
        private readonly IMongoCollection<UsuarioEstablecimiento> _mongoCollection;

        public TEstablecimientoUsersController(AsilosAncianosContext context, IMongoClient mongoClient)
        {
            _context = context;
            _mongoClient = mongoClient;
            _mongoCollection = _mongoClient.GetDatabase("AsilosAncianos").GetCollection<UsuarioEstablecimiento>("usuarios");
        }

        // GET: TEstablecimientoUsers
        public async Task<IActionResult> Index()
        {
            var users = _context.Establecimientos.Include(x => x.IdNavigation).Where(x => x.IdNavigation.Estado == 1);
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
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Insertar en SQL Server
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

                    // Insertar en MongoDB
                    UsuarioEstablecimiento estaM = new UsuarioEstablecimiento()
                    {
                        IdAux = u.Id,
                        nombre = tEstablecimientoUser.Nombre,
                        nit = tEstablecimientoUser.Nit,
                        representantePrincipal = tEstablecimientoUser.RepresentantePrincipal,
                        correoElectrónico = tEstablecimientoUser.Email,
                        telefono = tEstablecimientoUser.Telefono,
                        celular = tEstablecimientoUser.Celular,
                        direccion = tEstablecimientoUser.Direccion,
                        latitud = tEstablecimientoUser.Latitud,
                        longitud = tEstablecimientoUser.Longitud,
                        usuario = tEstablecimientoUser.Usuario1,
                        contraseña = tEstablecimientoUser.Password,
                        rol = "ESTABLECIMIENTO",
                        estado = 1,
                        fechaRegistro = DateTime.Now,
                        fechaActualizada = null
                    };

                    var database = _mongoClient.GetDatabase("AsilosAncianos");
                    var collection = database.GetCollection<UsuarioEstablecimiento>("usuarios");
                    collection.InsertOne(estaM);

                    transaction.Commit();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    transaction.Rollback();
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

                    var filter = Builders<UsuarioEstablecimiento>.Filter.Eq(e => e.IdAux, id);
                    var update = Builders<UsuarioEstablecimiento>.Update
                        .Set(e => e.nombre, tEstablecimientoUser.Nombre)
                        .Set(e => e.nit, tEstablecimientoUser.Nit)
                        .Set(e => e.representantePrincipal, tEstablecimientoUser.RepresentantePrincipal)
                        .Set(e => e.telefono, tEstablecimientoUser.Telefono)
                        .Set(e => e.celular, tEstablecimientoUser.Celular)
                        .Set(e => e.direccion, tEstablecimientoUser.Direccion)
                        .Set(e => e.latitud, tEstablecimientoUser.Latitud)
                        .Set(e => e.longitud, tEstablecimientoUser.Longitud);

                    var updateResult = await _mongoCollection.UpdateOneAsync(filter, update);

                    if (updateResult.ModifiedCount == 0)
                    {
                        // Manejar el caso cuando el documento no existe en MongoDB o no se modificó correctamente
                    }
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
                var update = Builders<UsuarioEstablecimiento>.Update.Set(r => r.estado, 0);

                var updateResult = await _mongoCollection.UpdateOneAsync(filter, update);

                if (updateResult.ModifiedCount == 0)
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                // Manejar cualquier excepción
            }

            return RedirectToAction("Index", "TEstablecimientoUsers");
        }

        private bool TEstablecimientoUserExists(int id)
        {
          return (_context.TEstablecimientoUser?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
