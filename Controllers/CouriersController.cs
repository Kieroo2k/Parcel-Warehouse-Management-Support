using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ParcelWarehouseManagementSupport.Models;

namespace ParcelWarehouseManagementSupport.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CouriersController : Controller
    {
        private readonly ParcelWarehouseDBContext _context;

        public CouriersController(ParcelWarehouseDBContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var allUsers = _context.Users.FromSqlRaw("SELECT * FROM ParcelWarehouse.dbo.AspNetUsers").ToList();

            foreach(var User in allUsers)
            {
                var courier = new Courier()
                {
                    CourierId = User.Id,
                    FirstName = User.FirstName,
                    LastName = User.LastName,
                    Email = User.Email
                };
                if (CourierExists(User.Id))
                {
                    continue;
                }
                else
                {
                    _context.Add(courier);
                    await _context.SaveChangesAsync();
                }
            }
            return View(await _context.Couriers.ToListAsync());
        }
        [HttpPost]
        public IActionResult Index(String query)
        {
            if(query != null)
            {
                var couriers = _context.Couriers.Where(courier => courier.LastName.Contains(query) || courier.FirstName.Contains(query)).ToList();
                return View(couriers);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }

        }

        public async Task<IActionResult> Details(String id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courier = await _context.Couriers
                .FirstOrDefaultAsync(m => m.CourierId == id);
            if (courier == null)
            {
                return NotFound();
            }

            return View(courier);
        }

        public async Task<ActionResult> Delete(String id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var Courier = await _context.Couriers.FirstOrDefaultAsync(m => m.CourierId == id);
            if (Courier == null)
            {
                return NotFound();
            }

            return View(Courier);
        }

        [HttpPost]
        public ActionResult Delete(String id, IFormCollection collection)
        {
            using (SqlConnection sqlCon = new SqlConnection("Server=DESKTOP-L9U3PG4\\SQLEXPRESS1;Database=ParcelWarehouse;Trusted_Connection=True;"))
            {
                sqlCon.Open();

                string query = "DELETE FROM ParcelWarehouse.dbo.AspNetUsers WHERE Id = @Id" +
                               " DELETE FROM ParcelWarehouse.dbo.Couriers WHERE CourierId = @Id";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@Id", id);
                sqlCmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        private bool CourierExists(String id)
        {
            return _context.Couriers.Any(e => e.CourierId == id);
        }

    }
}
