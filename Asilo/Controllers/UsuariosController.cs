using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Asilo.Data;
using Asilo.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace Asilo.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly AsilosAncianosContext _context;

        public UsuariosController(AsilosAncianosContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Usuario userf)
        {
            var userLogin = from usr in _context.Usuarios
                            where usr.Usuario1 == userf.Usuario1 && usr.Password == userf.Password
                            select new
                            {
                                userID = usr.Id,
                                Name = usr.Usuario1,
                                Password = usr.Password,
                                RolName = usr.Role
                            };
            if (userLogin.Count() != 0)
            {
                string rol = userLogin.First().RolName;
                string name = userLogin.First().Name;
                int id = userLogin.First().userID;
                var claims = new List<Claim>{
                    new Claim(ClaimTypes.Name, name),
                    new Claim(ClaimTypes.Role, rol),
                   new Claim(ClaimTypes.NameIdentifier, id.ToString())
                };
                var clainsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(clainsIdentity));
                return RedirectToAction("Index", "Home");
            
            }
            else
            {
                ViewData["message"] = "Nombre de Usuario/Contraseña incorrectos";
                return View();
            }
        }
        public async Task<IActionResult> LogOut()
        {
            //Matamos a la cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Usuarios");
        }
        // GET: Users1/Edit/5
        /*public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["Id"] = new SelectList(_context.People, "Id", "FirstName", user.Id);
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "RolName", user.RoleId);
            return View(user);
        }

        // POST: Users1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,Password,RoleId,Status,RegisterDate,UpdateDate,UserId")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    user.RegisterDate = DateTime.Now;
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            ViewData["Id"] = new SelectList(_context.People, "Id", "FirstName", user.Id);
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "rolName", user.RoleId);
            return View(user);
        }

        // GET: Users1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.IdNavigation)
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'dbBeatoSalomonContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.Status = 0;
                _context.Users.Update(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }*/
    }
}
