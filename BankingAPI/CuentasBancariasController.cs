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
    public class CuentasBancariasController : Controller
    {
        private readonly BankingAPIContext _context;

        public CuentasBancariasController(BankingAPIContext context)
        {
            _context = context;
        }

        // GET: CuentasBancarias
        public async Task<IActionResult> Index()
        {
            return View(await _context.CuentasBancarias.ToListAsync());
        }

        // GET: CuentasBancarias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cuentasBancarias = await _context.CuentasBancarias
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cuentasBancarias == null)
            {
                return NotFound();
            }

            return View(cuentasBancarias);
        }

        // GET: CuentasBancarias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CuentasBancarias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NumeroCuenta,Saldo,ClienteId")] CuentasBancarias cuentasBancarias)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cuentasBancarias);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cuentasBancarias);
        }

        // GET: CuentasBancarias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cuentasBancarias = await _context.CuentasBancarias.FindAsync(id);
            if (cuentasBancarias == null)
            {
                return NotFound();
            }
            return View(cuentasBancarias);
        }

        // POST: CuentasBancarias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NumeroCuenta,Saldo,ClienteId")] CuentasBancarias cuentasBancarias)
        {
            if (id != cuentasBancarias.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cuentasBancarias);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CuentasBancariasExists(cuentasBancarias.Id))
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
            return View(cuentasBancarias);
        }

        // GET: CuentasBancarias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cuentasBancarias = await _context.CuentasBancarias
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cuentasBancarias == null)
            {
                return NotFound();
            }

            return View(cuentasBancarias);
        }

        // POST: CuentasBancarias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cuentasBancarias = await _context.CuentasBancarias.FindAsync(id);
            if (cuentasBancarias != null)
            {
                _context.CuentasBancarias.Remove(cuentasBancarias);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CuentasBancariasExists(int id)
        {
            return _context.CuentasBancarias.Any(e => e.Id == id);
        }
    }
}
