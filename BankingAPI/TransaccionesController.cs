using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BankingAPI.Data;
using BankingAPI.Models;

namespace BankingAPI
{
    public class TransaccionesController : Controller
    {
        private readonly BankingAPIContext _context;

        public TransaccionesController(BankingAPIContext context)
        {
            _context = context;
        }

        // GET: Transacciones
        public async Task<IActionResult> Index()
        {
            return View(await _context.Transacciones.ToListAsync());
        }

        // GET: Transacciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transacciones = await _context.Transacciones
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transacciones == null)
            {
                return NotFound();
            }

            return View(transacciones);
        }

        // GET: Transacciones/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Transacciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Tipo,Monto,Fecha,SaldoPosterior,CuentaId")] Transacciones transacciones)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transacciones);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(transacciones);
        }

        // GET: Transacciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transacciones = await _context.Transacciones.FindAsync(id);
            if (transacciones == null)
            {
                return NotFound();
            }
            return View(transacciones);
        }

        // POST: Transacciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Tipo,Monto,Fecha,SaldoPosterior,CuentaId")] Transacciones transacciones)
        {
            if (id != transacciones.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transacciones);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransaccionesExists(transacciones.Id))
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
            return View(transacciones);
        }

        // GET: Transacciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transacciones = await _context.Transacciones
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transacciones == null)
            {
                return NotFound();
            }

            return View(transacciones);
        }

        // POST: Transacciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transacciones = await _context.Transacciones.FindAsync(id);
            if (transacciones != null)
            {
                _context.Transacciones.Remove(transacciones);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransaccionesExists(int id)
        {
            return _context.Transacciones.Any(e => e.Id == id);
        }
    }
}
