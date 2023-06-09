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
using MongoDB.Driver;
using System.Net;
using System.Net.Mail;
using Microsoft.SqlServer.Server;
using System.Collections;


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
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var asilosAncianosContext = _context.Donacions
                .Include(d => d.Benefactor)
                .Include(d => d.Campana)
                .Include(d => d.Recolector)
                .Where(x => x.Campana.EstablecimientoId == int.Parse(userId))
                .OrderByDescending(d => d.Fecha); // Ordenar por la fecha en forma descendente

            return View(await asilosAncianosContext.ToListAsync());
        }
        public async Task<IActionResult> ListRecoec()
        {
            var asilosAncianosContext = _context.Donacions.Include(d => d.Benefactor).Include(d => d.Campana)
           .Include(d => d.Recolector).Where(x => x.Recibida == false);
            return View(await asilosAncianosContext.ToListAsync());

        }
        public async Task<IActionResult> ReporteDonaciones(DateTime fechaInicio, DateTime fechaFin)
        {
            var donaciones = await _context.Donacions
             .Where(d => d.Recibida == true && d.Fecha >= fechaInicio && d.Fecha <= fechaFin)
             .OrderBy(d => d.Fecha)
             .Include(d => d.Benefactor)
             .ToListAsync();

            var model = new Tuple<DateTime, DateTime, List<Donacion>>(fechaInicio, fechaFin, donaciones);

            return View(model); ;
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
            ViewData["BenefactorId"] = new SelectList(_context.Benefactors, "Id", "Nombres");
            ViewData["CampanaId"] = new SelectList(_context.Campanas, "Id", "Nombre");
            ViewData["RecolectorId"] = new SelectList(_context.Recolectors, "Id", "Nonbre");
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


       
            public async Task<IActionResult> MandarCorreo(int? id)
        {
            try
            {
                // Configurar los datos de autenticación de Gmail
                var smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential("brayancm603@gmail.com", "uhdubprsoycffzwg");
                smtpClient.EnableSsl = true;

                // Crear el mensaje de correo
                var message = new MailMessage();
                message.From = new MailAddress("brayancm603@gmail.com");
                message.To.Add(new MailAddress("brayancm603@gmail.com"));
                message.Subject = "Recolector enviado";
                message.Body = "Mandamos a un recolector de confianza a tu dirección para el recojo de la donación";

                // Enviar el correo electrónico
                await smtpClient.SendMailAsync(message);
                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var esta = _context.Establecimientos
                  .Where(x => x.Campanas.FirstOrDefault() != null && x.Campanas.First().Donacions.FirstOrDefault() != null && x.Id == x.Campanas.First().Donacions.First().RecolectorId)
                  .FirstOrDefault();
                int result = esta != null ? esta.Id : 2;

                int recolectorId;
                if (int.TryParse(userId, out recolectorId))
                {
                    RecojosRealizado r = new RecojosRealizado()
                    {
                        EstablecimientoId = result,
                        RecolectorId = recolectorId,
                        Cantidad = 2,
                        Fecha = DateTime.Now,
                    };
                    _context.RecojosRealizados.Add(r);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Manejar el caso en el que userId no pueda convertirse a un entero válido
                    // Puedes lanzar una excepción, mostrar un mensaje de error, etc.
                }
                var idDona = await _context.Donacions.FindAsync(id);
                if (idDona != null && id == idDona.Id)
                {
                    idDona.Recibida = true;
                    _context.Donacions.Update(idDona);
                    await _context.SaveChangesAsync();
                }


                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                // Manejar cualquier excepción
            }

            return RedirectToAction("Index", "Home");
        }
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
