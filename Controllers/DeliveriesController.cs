using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParcelWarehouseManagementSupport.Models;
using ParcelWarehouseManagementSupport.Data;
using ParcelWarehouseManagementSupport.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Web.Helpers;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json;

namespace ParcelWarehouseManagementSupport.Controllers
{
    [Authorize]
    public class DeliveriesController : Controller
    {
        private readonly ParcelWarehouseDBContext _context;

        public DeliveriesController(ParcelWarehouseDBContext context, AuthDbContext _context2)
        {
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }
        public string ErrorMessage { get; set; }


        // GET: Deliveries
        public async Task<IActionResult> Index()
        {
            List<Transmission> transmissions = new List<Transmission>();
            transmissions = await _context.Transmissions.FromSqlRaw("SELECT * FROM ParcelWarehouse.dbo.Transmissions").ToListAsync();

            ViewData["Transmissions"] = transmissions;

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var couriers = _context.Couriers.FromSqlRaw("SELECT * FROM ParcelWarehouse.dbo.Couriers").ToList();
            var magnus = _context.Deliveries.FromSqlRaw("SELECT * FROM ParcelWarehouse.dbo.Deliveries WHERE CourierId = '75f658b8-99de-4b47-a5c2-db4528e9fe63'").ToList();
            var caruana = _context.Deliveries.FromSqlRaw("SELECT * FROM ParcelWarehouse.dbo.Deliveries WHERE CourierId = 'f5b267ee-7f95-4f85-90b4-89e21056ed66'").ToList();
            var magnus1 = magnus.Count();
            var caruana1 = caruana.Count();

            ViewBag.ActualUserId = userId;
            ViewBag.Couriers = couriers;
            ViewBag.Magnus = magnus1;
            ViewBag.Caruana = caruana1;

            if (User.IsInRole("Admin"))
            {
                return View(await _context.Deliveries.ToListAsync());
            }
            else
            {
                string usr = User.Identity.Name;
                var CourierDeliveries = await _context.Deliveries.FromSqlRaw("SELECT DeliveryId,DeliveryNumber,ParcelsNUMBER,Deliveries.CourierId,SenderId,RecipientId,TotalWeight,DestinationId,Fee,Paid,Email FROM ParcelWarehouse.dbo.Deliveries INNER JOIN ParcelWarehouse.dbo.Couriers ON Deliveries.CourierId = Couriers.CourierId WHERE Couriers.Email = '" + usr + "'").ToListAsync();
                return View(CourierDeliveries.ToList());
            }

        }
        [HttpPost]
        public IActionResult Index(int? id, String Transmite, String Claim, String Return)
        {
            DateTime actualDate = DateTime.Now;
            var actualDateString = actualDate.ToString();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var transmission = _context.Deliveries.Find(id);
            var transmissions = _context.Transmissions.Find(id);
            var returns = _context.Returns.Find(id);

            var returnList = _context.Returns.ToList();

            if (!String.IsNullOrEmpty(Transmite))
            {
                    if (transmissions == null)
                    {
                        _context.Database.ExecuteSqlRaw("INSERT INTO ParcelWarehouse.dbo.Transmissions (TransmissionId,DeliveryId,DeliveryNumber,ParcelsNUMBER,CourierId,SenderId,RecipientId,TotalWeight,DestinationId,Fee,Paid) VALUES (" + transmission.DeliveryId + "," + transmission.DeliveryId + "," + transmission.DeliveryNumber + "," + transmission.ParcelsNUMBER + ",\'" + transmission.CourierId + "\'," + transmission.SenderId + "," + transmission.RecipientId + "," + transmission.TotalWeight + "," + transmission.DestinationId + "," + transmission.Fee + ",\'" + transmission.Paid + "\')");
                        _context.SaveChanges();
                    }
                    else
                    {
                        TempData["ErrorMes"] = "Transmission already exists";
                        return RedirectToAction("Index", "Deliveries");
                    }

            }
            else if(!String.IsNullOrEmpty(Claim))
            {
                    if (userId == transmissions.courier.CourierId)
                    {
                        _context.Database.ExecuteSqlRaw("DELETE FROM ParcelWarehouse.dbo.Transmissions WHERE TransmissionId = " + "\'" + transmissions.TransmissionId + "\'");
                        _context.SaveChanges();
                    }
                    else
                    {
                        _context.Database.ExecuteSqlRaw("UPDATE ParcelWarehouse.dbo.Deliveries SET CourierId = " + "\'" + userId  + "\'" + "WHERE DeliveryId = " + "\'" + transmission.DeliveryId + "\'");
                        _context.Database.ExecuteSqlRaw("DELETE FROM ParcelWarehouse.dbo.Transmissions WHERE TransmissionId = " + "\'" + transmissions.TransmissionId + "\'");
                        _context.SaveChanges();
                    }

            }
            else if(!String.IsNullOrEmpty(Return))
            {
                    if (returns == null)
                    {
                        _context.Database.ExecuteSqlRaw("INSERT INTO ParcelWarehouse.dbo.Returns (ReturnId,CourierId,ParcelsNUMBER,DeliveryId,DateAdded) VALUES (" + transmission.DeliveryId + ",\'" + transmission.CourierId + "\'," + transmission.ParcelsNUMBER + "," + transmission.DeliveryId + ",\'" + actualDateString + "\')");
                        _context.SaveChanges();
                        return RedirectToAction("Edit", "Returns", new { @id = id });
                    }
                    else
                    {
                        return RedirectToAction("Edit", "Returns", new { @id = id });
                    }    
            }
         
            return RedirectToAction(nameof(Index));
        }

        // GET: Deliveries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var delivery = await _context.Deliveries
                .FirstOrDefaultAsync(m => m.DeliveryId == id);
            if (delivery == null)
            {
                return NotFound();
            }

            return View(delivery);
        }
        public async Task<IActionResult> Transmite(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var delivery = await _context.Deliveries
                .FirstOrDefaultAsync(m => m.DeliveryId == id);
            if (delivery == null)
            {
                return NotFound();
            }
            var DeliveriesList = new List<Delivery>();
            DeliveriesList.Add(delivery);
            return RedirectToAction("Index", "Deliveries");
        }

        // GET: Deliveries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var delivery = await _context.Deliveries.FindAsync(id);
            if (delivery == null)
            {
                return NotFound();
            }
            return View(delivery);
        }

        // POST: Deliveries/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DeliveryId,DeliveryNumber,ParcelsNUMBER,CourierId,SenderId,RecipientId,TotalWeight,DestinationId,Fee,Paid")] Delivery delivery)
        {
            if (id != delivery.DeliveryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(delivery);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeliveryExists(delivery.DeliveryId))
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
            return View(delivery);
        }

        // GET: Deliveries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var delivery = await _context.Deliveries
                .FirstOrDefaultAsync(m => m.DeliveryId == id);
            if (delivery == null)
            {
                return NotFound();
            }

            return View(delivery);
        }

        // POST: Deliveries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var delivery = await _context.Deliveries.FindAsync(id);
            _context.Deliveries.Remove(delivery);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeliveryExists(int id)
        {
            return _context.Deliveries.Any(e => e.DeliveryId == id);
        }
    }
}
