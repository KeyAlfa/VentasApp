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
    public class EntradasController : Controller
    {
        private readonly VentasAppContext _context;

        public EntradasController(VentasAppContext context)
        {
            _context = context;
        }

        // GET: Entradas
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Verifica si el usuario tiene el rol de "Administrator"
            var isAdmin = User.IsInRole("Administrator");

            // Filtra las entradas en función del ID del usuario si no es administrador
            var ventasAppContext = _context.Entrada
                .Where(e => isAdmin || e.UsuarioId == userId)
                .Include(e => e.Evento)
                .Include(e => e.Usuario);

            // Devuelve la lista filtrada de entradas a la vista
            return View(await ventasAppContext.ToListAsync());
        }

        // GET: Entradas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Entrada == null)
            {
                return NotFound();
            }

            var entrada = await _context.Entrada
                .Include(e => e.Evento)
                .Include(e => e.Usuario)
                .FirstOrDefaultAsync(m => m.EntradaId == id);
            if (entrada == null)
            {
                return NotFound();
            }

            return View(entrada);
        }

        // GET: Entradas/Create
        public IActionResult Create()
        {
            ViewData["EventoId"] = new SelectList(_context.Eventos, "EventoId", "NombreEvento");

            // Check if the user has the "Admin" role
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

        // POST: Entradas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EntradaId,EventoId,UsuarioId")] Entrada entrada)
        {
            try
            {
                _context.Add(entrada);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                throw;
            }

            ViewData["EventoId"] = new SelectList(_context.Eventos, "EventoId", "EventoId", entrada.EventoId);

            // Check if the user has the "Admin" role
            if (User.IsInRole("Administrator"))
            {
                ViewData["UsuarioId"] = new SelectList(_context.AspNetUsers, "Id", "UserName");
            }
            else
            {
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                ViewData["UsuarioId"] = new SelectList(_context.AspNetUsers.Where(u => u.Id == currentUserId), "Id", "UserName");
                entrada.UsuarioId = currentUserId; // Set the UsuarioId to the current user's ID
            }

            return View(entrada);
        }

        // GET: Entradas/Edit/5
        [Authorize(Roles = "Administrator,Soporte")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Entrada == null)
            {
                return NotFound();
            }

            var entrada = await _context.Entrada.FindAsync(id);
            if (entrada == null)
            {
                return NotFound();
            }
            ViewData["EventoId"] = new SelectList(_context.Eventos, "EventoId", "NombreEvento", entrada.EventoId);
            ViewData["UsuarioId"] = new SelectList(_context.AspNetUsers, "Id", "UserName", entrada.UsuarioId);
            return View(entrada);
        }

        // POST: Entradas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator,Soporte")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EntradaId,EventoId,UsuarioId")] Entrada entrada)
        {
            if (id != entrada.EntradaId)
            {
                return NotFound();
            }         
                try
                {
                    _context.Update(entrada);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntradaExists(entrada.EntradaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            
            ViewData["EventoId"] = new SelectList(_context.Eventos, "EventoId", "EventoId", entrada.EventoId);
            ViewData["UsuarioId"] = new SelectList(_context.AspNetUsers, "Id", "Id", entrada.UsuarioId);
            return View(entrada);
        }

        // GET: Entradas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Entrada == null)
            {
                return NotFound();
            }

            var entrada = await _context.Entrada
                .Include(e => e.Evento)
                .Include(e => e.Usuario)
                .FirstOrDefaultAsync(m => m.EntradaId == id);
            if (entrada == null)
            {
                return NotFound();
            }

            return View(entrada);
        }

        // POST: Entradas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Entrada == null)
            {
                return Problem("Entity set 'VentasAppContext.Entrada'  is null.");
            }
            var entrada = await _context.Entrada.FindAsync(id);
            if (entrada != null)
            {
                _context.Entrada.Remove(entrada);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EntradaExists(int id)
        {
            return (_context.Entrada?.Any(e => e.EntradaId == id)).GetValueOrDefault();
        }
    }
}
