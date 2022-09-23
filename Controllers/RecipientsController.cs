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
    public class RecipientsController : Controller
    {
        private readonly ParcelWarehouseDBContext _context;

        public RecipientsController(ParcelWarehouseDBContext context)
        {
            _context = context;
        }

        // GET: Recipients
        public async Task<IActionResult> Index()
        {
            return View(await _context.Recipients.ToListAsync());
        }

        // GET: Recipients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipient = await _context.Recipients
                .FirstOrDefaultAsync(m => m.RecipientId == id);
            if (recipient == null)
            {
                return NotFound();
            }

            return View(recipient);
        }

        // GET: Recipients/Create
    /*    public IActionResult Create()
        {
            return View();
        }

        // POST: Recipients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RecipientId,FirstName,LastName, Email, FullName")] Recipient recipient)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recipient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(recipient);
        }
*/
        // GET: Recipients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipient = await _context.Recipients.FindAsync(id);
            if (recipient == null)
            {
                return NotFound();
            }
            return View(recipient);
        }

        // POST: Recipients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RecipientId,FirstName,LastName, Email, FullName")] Recipient recipient)
        {
            if (id != recipient.RecipientId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipientExists(recipient.RecipientId))
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
            return View(recipient);
        }

        // GET: Recipients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipient = await _context.Recipients
                .FirstOrDefaultAsync(m => m.RecipientId == id);
            if (recipient == null)
            {
                return NotFound();
            }

            return View(recipient);
        }

        // POST: Recipients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recipient = await _context.Recipients.FindAsync(id);
            _context.Recipients.Remove(recipient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecipientExists(int id)
        {
            return _context.Recipients.Any(e => e.RecipientId == id);
        }
    }
}
