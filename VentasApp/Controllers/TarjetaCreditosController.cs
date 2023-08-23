using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VentasApp.Models;

namespace VentasApp.Controllers
{
    [Authorize]
    public class TarjetaCreditosController : Controller
    {
        private readonly VentasAppContext _context;

        public TarjetaCreditosController(VentasAppContext context)
        {
            _context = context;
        }

        // GET: TarjetaCreditos
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Verifica si el usuario tiene el rol de "Administrator"
            var isAdmin = User.IsInRole("Administrator");

            // Filtra las entradas en función del ID del usuario si no es administrador
            var ventasAppContext = _context.TarjetaCreditos
                .Where(e => isAdmin || e.UsuarioId == userId)
                .Include(t => t.Usuario);

            // Devuelve la lista filtrada de entradas a la vista
            return View(await ventasAppContext.ToListAsync());
        }

        // GET: TarjetaCreditos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TarjetaCreditos == null)
            {
                return NotFound();
            }

            var tarjetaCredito = await _context.TarjetaCreditos
                .Include(t => t.Usuario)
                .FirstOrDefaultAsync(m => m.TarjetaCreditoId == id);
            if (tarjetaCredito == null)
            {
                return NotFound();
            }

            return View(tarjetaCredito);
        }

        // GET: TarjetaCreditos/Create
        public IActionResult Create()
        {
            if (User.IsInRole("Administrator"))
            {
                ViewData["UsuarioId"] = new SelectList(_context.AspNetUsers, "Id", "UserName");
            }
            else
            {
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                ViewData["UsuarioId"] = new SelectList(_context.AspNetUsers.Where(u => u.Id == currentUserId), "Id", "UserName");
            }

            return View();
        }

        // POST: TarjetaCreditos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TarjetaCreditoId,UsuarioId,Saldo")] TarjetaCredito tarjetaCredito)
        {
            try
            {
                _context.Add(tarjetaCredito);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {

                throw;
            }
                          
            ViewData["UsuarioId"] = new SelectList(_context.AspNetUsers, "Id", "UserName", tarjetaCredito.UsuarioId);
            // Check if the user has the "Admin" role
            if (User.IsInRole("Administrator"))
            {
                ViewData["UsuarioId"] = new SelectList(_context.AspNetUsers, "Id", "UserName");
            }
            else
            {
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                ViewData["UsuarioId"] = new SelectList(_context.AspNetUsers.Where(u => u.Id == currentUserId), "Id", "UserName");
                tarjetaCredito.UsuarioId = currentUserId; // Set the UsuarioId to the current user's ID
            }
            return View(tarjetaCredito);
        }

        // GET: TarjetaCreditos/Edit/5
        [Authorize(Roles = "Administrator,Soporte")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TarjetaCreditos == null)
            {
                return NotFound();
            }

            var tarjetaCredito = await _context.TarjetaCreditos.FindAsync(id);
            if (tarjetaCredito == null)
            {
                return NotFound();
            }
            ViewData["UsuarioId"] = new SelectList(_context.AspNetUsers, "Id", "UserName", tarjetaCredito.UsuarioId);
            return View(tarjetaCredito);
        }

        // POST: TarjetaCreditos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator,Soporte")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TarjetaCreditoId,UsuarioId,Saldo")] TarjetaCredito tarjetaCredito)
        {
            if (id != tarjetaCredito.TarjetaCreditoId)
            {
                return NotFound();
            }          
                try
                {
                    _context.Update(tarjetaCredito);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TarjetaCreditoExists(tarjetaCredito.TarjetaCreditoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            
            ViewData["UsuarioId"] = new SelectList(_context.AspNetUsers, "Id", "Id", tarjetaCredito.UsuarioId);
            return View(tarjetaCredito);
        }

        // GET: TarjetaCreditos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TarjetaCreditos == null)
            {
                return NotFound();
            }

            var tarjetaCredito = await _context.TarjetaCreditos
                .Include(t => t.Usuario)
                .FirstOrDefaultAsync(m => m.TarjetaCreditoId == id);
            if (tarjetaCredito == null)
            {
                return NotFound();
            }

            return View(tarjetaCredito);
        }

        // POST: TarjetaCreditos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TarjetaCreditos == null)
            {
                return Problem("Entity set 'VentasAppContext.TarjetaCreditos'  is null.");
            }
            var tarjetaCredito = await _context.TarjetaCreditos.FindAsync(id);
            if (tarjetaCredito != null)
            {
                _context.TarjetaCreditos.Remove(tarjetaCredito);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TarjetaCreditoExists(int id)
        {
          return (_context.TarjetaCreditos?.Any(e => e.TarjetaCreditoId == id)).GetValueOrDefault();
        }
    }
}
