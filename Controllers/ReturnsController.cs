using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ParcelWarehouseManagementSupport;
using ParcelWarehouseManagementSupport.Models;

namespace ParcelWarehouseManagementSupport.Controllers
{
    [Authorize]
    public class ReturnsController : Controller
    {
        private readonly ParcelWarehouseDBContext _context;

        public ReturnsController(ParcelWarehouseDBContext context)
        {
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }
        public string ErrorMessage { get; set; }

        // GET: Returns
        public async Task<IActionResult> Index()
        {
            return View(await _context.Returns.ToListAsync());
        }

        // GET: Returns/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @return = await _context.Returns
                .FirstOrDefaultAsync(m => m.ReturnId == id);
            if (@return == null)
            {
                return NotFound();
            }

            return View(@return);
        }

        // GET: Returns/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Returns/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReturnId,Description,CourierId,ParcelsNUMBER,DeliveryId,DateAdded")] Return @return)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@return);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@return);
        }

        // GET: Returns/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var @return = await _context.Returns.FindAsync(id);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (userId == @return.CourierId)
            {
                if (id == null)
                {
                    return NotFound();
                }
                if (@return == null)
                {
                    return NotFound();
                }
                return View(@return);
            }
            else
            {
                TempData["ErrorMess"] = "You can edit only your returns";
                return RedirectToAction("Details", "Returns", new { @id = id });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReturnId,Description,CourierId,ParcelsNUMBER,DeliveryId,DateAdded")] Return @return)
        {
            var transmission = _context.Deliveries.Find(id);

                if (id != @return.ReturnId)
                {
                    return NotFound();
                }

            if (ModelState.IsValid)
            {
                if (@return.ParcelsNUMBER <= transmission.ParcelsNUMBER && @return.ParcelsNUMBER > 0)
                {
                    try
                    {
                        _context.Update(@return);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ReturnExists(@return.ReturnId))
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
                else
                {
                    TempData["ErrorMess"] = "Number of Parcels isn't correct";
                    return RedirectToAction("Edit", "Returns");
                }
            }
            return View(@return);

        }

        // GET: Returns/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @return = await _context.Returns
                .FirstOrDefaultAsync(m => m.ReturnId == id);
            if (@return == null)
            {
                return NotFound();
            }

            return View(@return);
        }

        // POST: Returns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @return = await _context.Returns.FindAsync(id);
            _context.Returns.Remove(@return);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReturnExists(int id)
        {
            return _context.Returns.Any(e => e.ReturnId == id);
        }
    }
}
