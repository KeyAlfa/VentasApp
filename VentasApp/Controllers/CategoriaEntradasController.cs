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
    public class CategoriaEntradasController : Controller
    {
        private readonly VentasAppContext _context;

        public CategoriaEntradasController(VentasAppContext context)
        {
            _context = context;
        }

        // GET: CategoriaEntradas
        public async Task<IActionResult> Index()
        {
              return _context.CategoriaEntrada != null ? 
                          View(await _context.CategoriaEntrada.ToListAsync()) :
                          Problem("Entity set 'VentasAppContext.CategoriaEntrada'  is null.");
        }

        // GET: CategoriaEntradas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CategoriaEntrada == null)
            {
                return NotFound();
            }

            var categoriaEntrada = await _context.CategoriaEntrada
                .FirstOrDefaultAsync(m => m.CategoriaEntradaId == id);
            if (categoriaEntrada == null)
            {
                return NotFound();
            }

            return View(categoriaEntrada);
        }

        // GET: CategoriaEntradas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CategoriaEntradas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoriaEntradaId,Nombre,Precio,Capacidad")] CategoriaEntrada categoriaEntrada)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoriaEntrada);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoriaEntrada);
        }

        // GET: CategoriaEntradas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CategoriaEntrada == null)
            {
                return NotFound();
            }

            var categoriaEntrada = await _context.CategoriaEntrada.FindAsync(id);
            if (categoriaEntrada == null)
            {
                return NotFound();
            }
            return View(categoriaEntrada);
        }

        // POST: CategoriaEntradas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoriaEntradaId,Nombre,Precio,Capacidad")] CategoriaEntrada categoriaEntrada)
        {
            if (id != categoriaEntrada.CategoriaEntradaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoriaEntrada);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriaEntradaExists(categoriaEntrada.CategoriaEntradaId))
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
            return View(categoriaEntrada);
        }

        // GET: CategoriaEntradas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CategoriaEntrada == null)
            {
                return NotFound();
            }

            var categoriaEntrada = await _context.CategoriaEntrada
                .FirstOrDefaultAsync(m => m.CategoriaEntradaId == id);
            if (categoriaEntrada == null)
            {
                return NotFound();
            }

            return View(categoriaEntrada);
        }

        // POST: CategoriaEntradas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CategoriaEntrada == null)
            {
                return Problem("Entity set 'VentasAppContext.CategoriaEntrada'  is null.");
            }
            var categoriaEntrada = await _context.CategoriaEntrada.FindAsync(id);
            if (categoriaEntrada != null)
            {
                _context.CategoriaEntrada.Remove(categoriaEntrada);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoriaEntradaExists(int id)
        {
          return (_context.CategoriaEntrada?.Any(e => e.CategoriaEntradaId == id)).GetValueOrDefault();
        }
    }
}
