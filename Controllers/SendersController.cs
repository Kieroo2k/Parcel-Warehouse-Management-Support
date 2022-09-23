using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ParcelWarehouseManagementSupport;
using ParcelWarehouseManagementSupport.Models;

namespace ParcelWarehouseManagementSupport.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SendersController : Controller
    {
        private readonly ParcelWarehouseDBContext _context;

        public SendersController(ParcelWarehouseDBContext context)
        {
            _context = context;
        }

        // GET: Senders
        public async Task<IActionResult> Index()
        {
            return View(await _context.Senders.ToListAsync());
        }

        // GET: Senders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sender = await _context.Senders
                .FirstOrDefaultAsync(m => m.SenderId == id);
            if (sender == null)
            {
                return NotFound();
            }

            return View(sender);
        }

        // GET: Senders/Create
  /*      public IActionResult Create()
        {
            return View();
        }

        // POST: Senders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SenderId,FirstName,LastName")] Sender sender)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sender);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sender);
        }
  */
        // GET: Senders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sender = await _context.Senders.FindAsync(id);
            if (sender == null)
            {
                return NotFound();
            }
            return View(sender);
        }

        // POST: Senders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SenderId,FirstName,LastName")] Sender sender)
        {
            if (id != sender.SenderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sender);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SenderExists(sender.SenderId))
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
            return View(sender);
        }

        // GET: Senders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sender = await _context.Senders
                .FirstOrDefaultAsync(m => m.SenderId == id);
            if (sender == null)
            {
                return NotFound();
            }

            return View(sender);
        }

        // POST: Senders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sender = await _context.Senders.FindAsync(id);
            _context.Senders.Remove(sender);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SenderExists(int id)
        {
            return _context.Senders.Any(e => e.SenderId == id);
        }
    }
}
