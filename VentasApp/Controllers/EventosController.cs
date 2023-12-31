﻿using System;
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
    [Authorize(Roles = "Usuario,Administrator,Soporte")]
    public class EventosController : Controller
    {
        private readonly VentasAppContext _context;

        public EventosController(VentasAppContext context)
        {
            _context = context;
        }

        // GET: Eventos
        [Authorize(Roles = "Usuario,Administrator,Soporte")]
        public async Task<IActionResult> Index()
        {
            return _context.Eventos != null ? 
                          View(await _context.Eventos.ToListAsync()) :
                          Problem("Entity set 'VentasAppContext.Eventos'  is null.");
        }

        // GET: Eventos/Details/5
        [Authorize(Roles = "Usuario,Administrator,Soporte")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Eventos == null)
            {
                return NotFound();
            }

            var evento = await _context.Eventos
                .FirstOrDefaultAsync(m => m.EventoId == id);
            if (evento == null)
            {
                return NotFound();
            }

            return View(evento);
        }

        // GET: Eventos/Create
        [Authorize(Roles = "Administrator,Soporte")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Eventos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Soporte")]
        public async Task<IActionResult> Create([Bind("EventoId,NombreEvento,Descripcion,Ubicacion,Fecha")] Evento evento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(evento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(evento);
        }

        // GET: Eventos/Edit/5
        [Authorize(Roles = "Administrator,Soporte")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Eventos == null)
            {
                return NotFound();
            }

            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null)
            {
                return NotFound();
            }
            return View(evento);
        }

        // POST: Eventos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Soporte")]
        public async Task<IActionResult> Edit(int id, [Bind("EventoId,NombreEvento,Descripcion,Ubicacion,Fecha")] Evento evento)
        {
            if (id != evento.EventoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(evento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventoExists(evento.EventoId))
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
            return View(evento);
        }

        // GET: Eventos/Delete/5
        [Authorize(Roles = "Administrator,Soporte")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Eventos == null)
            {
                return NotFound();
            }

            var evento = await _context.Eventos
                .FirstOrDefaultAsync(m => m.EventoId == id);
            if (evento == null)
            {
                return NotFound();
            }

            return View(evento);

        }

        // POST: Eventos/Delete/5
        [Authorize(Roles = "Administrator,Soporte")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]      
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Eventos == null)
            {
                return Problem("Entity set 'VentasAppContext.Eventos'  is null.");
            }
            var evento = await _context.Eventos.FindAsync(id);
            if (evento != null)
            {
                _context.Eventos.Remove(evento);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventoExists(int id)
        {
          return (_context.Eventos?.Any(e => e.EventoId == id)).GetValueOrDefault();
        }
    }
}
