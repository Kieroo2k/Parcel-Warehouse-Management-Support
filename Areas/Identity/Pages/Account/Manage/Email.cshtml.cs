using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using ParcelWarehouseManagementSupport.Areas.Identity.Data;
using ParcelWarehouseManagementSupport.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ParcelWarehouseManagementSupport.Models;
using Microsoft.Data.SqlClient;

namespace ParcelWarehouseManagementSupport.Areas.Identity.Pages.Account.Manage
{
    public partial class EmailModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ParcelWarehouseDBContext _context;

        public EmailModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ParcelWarehouseDBContext _context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this._context = _context;
        }

        public string Username { get; set; }

        public string Email { get; set; }

    //    public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "New email")]
            public string NewEmail { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var email = await _userManager.GetEmailAsync(user);
            Email = email;

            Input = new InputModel
            {
                NewEmail = email,
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }


            var email = await _userManager.GetEmailAsync(user);
            var username = await _userManager.GetUserNameAsync(user);

            var courier = await _context.Couriers.FirstOrDefaultAsync(m => m.Email == user.Email);

            var allCouriers = await _context.Couriers.FromSqlRaw("SELECT * FROM ParcelWarehouse.dbo.Couriers").ToListAsync();

            if (Input.NewEmail != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, Input.NewEmail);
                var setUserNameResult = await _userManager.SetUserNameAsync(user, Input.NewEmail);

                courier.Email = Input.NewEmail;
                await _context.SaveChangesAsync();

                if (!setEmailResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting email for user with ID '{userId}'.");
                }
                StatusMessage = "Your Email is changed to " + Input.NewEmail;
            }

            await _signInManager.RefreshSignInAsync(user);
            return RedirectToPage();
        }

    
    }
}
