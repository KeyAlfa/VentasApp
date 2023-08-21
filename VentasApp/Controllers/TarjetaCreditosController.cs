using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VentasApp.Models;

namespace VentasApp.Controllers
{
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
            var ventasAppContext = _context.TarjetaCreditos.Include(t => t.Usuario);
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
            ViewData["UsuarioId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }

        // POST: TarjetaCreditos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TarjetaCreditoId,UsuarioId,Saldo")] TarjetaCredito tarjetaCredito)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tarjetaCredito);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioId"] = new SelectList(_context.AspNetUsers, "Id", "Id", tarjetaCredito.UsuarioId);
            return View(tarjetaCredito);
        }

        // GET: TarjetaCreditos/Edit/5
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
            ViewData["UsuarioId"] = new SelectList(_context.AspNetUsers, "Id", "Id", tarjetaCredito.UsuarioId);
            return View(tarjetaCredito);
        }

        // POST: TarjetaCreditos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TarjetaCreditoId,UsuarioId,Saldo")] TarjetaCredito tarjetaCredito)
        {
            if (id != tarjetaCredito.TarjetaCreditoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
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
            }
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
