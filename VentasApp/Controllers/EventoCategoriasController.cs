using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VentasApp.Models;

namespace VentasApp.Controllers
{
    [Authorize(Policy = "Administrator,Soporte")]
    public class EventoCategoriasController : Controller
    {
        private readonly VentasAppContext _context;

        public EventoCategoriasController(VentasAppContext context)
        {
            _context = context;
        }

        // GET: EventoCategorias
        public async Task<IActionResult> Index()
        {
            var ventasAppContext = _context.EventoCategoria.Include(e => e.CategoriaEntrada).Include(e => e.Evento);
            return View(await ventasAppContext.ToListAsync());
        }

        // GET: EventoCategorias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EventoCategoria == null)
            {
                return NotFound();
            }

            var eventoCategoria = await _context.EventoCategoria
                .Include(e => e.CategoriaEntrada)
                .Include(e => e.Evento)
                .FirstOrDefaultAsync(m => m.EventoCategoriaId == id);
            if (eventoCategoria == null)
            {
                return NotFound();
            }

            return View(eventoCategoria);
        }

        // GET: EventoCategorias/Create
        public IActionResult Create()
        {
            ViewData["CategoriaEntradaId"] = new SelectList(_context.CategoriaEntrada, "CategoriaEntradaId", "CategoriaEntradaId");
            ViewData["EventoId"] = new SelectList(_context.Eventos, "EventoId", "EventoId");
            return View();
        }

        // POST: EventoCategorias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventoCategoriaId,CategoriaEntradaId,EventoId")] EventoCategoria eventoCategoria)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eventoCategoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaEntradaId"] = new SelectList(_context.CategoriaEntrada, "CategoriaEntradaId", "CategoriaEntradaId", eventoCategoria.CategoriaEntradaId);
            ViewData["EventoId"] = new SelectList(_context.Eventos, "EventoId", "EventoId", eventoCategoria.EventoId);
            return View(eventoCategoria);
        }

        // GET: EventoCategorias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.EventoCategoria == null)
            {
                return NotFound();
            }

            var eventoCategoria = await _context.EventoCategoria.FindAsync(id);
            if (eventoCategoria == null)
            {
                return NotFound();
            }
            ViewData["CategoriaEntradaId"] = new SelectList(_context.CategoriaEntrada, "CategoriaEntradaId", "CategoriaEntradaId", eventoCategoria.CategoriaEntradaId);
            ViewData["EventoId"] = new SelectList(_context.Eventos, "EventoId", "EventoId", eventoCategoria.EventoId);
            return View(eventoCategoria);
        }

        // POST: EventoCategorias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventoCategoriaId,CategoriaEntradaId,EventoId")] EventoCategoria eventoCategoria)
        {
            if (id != eventoCategoria.EventoCategoriaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventoCategoria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventoCategoriaExists(eventoCategoria.EventoCategoriaId))
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
            ViewData["CategoriaEntradaId"] = new SelectList(_context.CategoriaEntrada, "CategoriaEntradaId", "CategoriaEntradaId", eventoCategoria.CategoriaEntradaId);
            ViewData["EventoId"] = new SelectList(_context.Eventos, "EventoId", "EventoId", eventoCategoria.EventoId);
            return View(eventoCategoria);
        }

        // GET: EventoCategorias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.EventoCategoria == null)
            {
                return NotFound();
            }

            var eventoCategoria = await _context.EventoCategoria
                .Include(e => e.CategoriaEntrada)
                .Include(e => e.Evento)
                .FirstOrDefaultAsync(m => m.EventoCategoriaId == id);
            if (eventoCategoria == null)
            {
                return NotFound();
            }

            return View(eventoCategoria);
        }

        // POST: EventoCategorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.EventoCategoria == null)
            {
                return Problem("Entity set 'VentasAppContext.EventoCategoria'  is null.");
            }
            var eventoCategoria = await _context.EventoCategoria.FindAsync(id);
            if (eventoCategoria != null)
            {
                _context.EventoCategoria.Remove(eventoCategoria);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventoCategoriaExists(int id)
        {
          return (_context.EventoCategoria?.Any(e => e.EventoCategoriaId == id)).GetValueOrDefault();
        }
    }
}
