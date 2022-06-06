// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using KGB_Dev_.Data;
using KGB_Dev_.Data.KGB_Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace KGB_Dev_.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly ApplicationDbContext _context;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }
        [BindProperty]
        public KGB_User Input { get; set; }
        public Dictionary<int, string> ListOfOrg { get; set; }
        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public async Task OnGetAsync(string returnUrl = null)
        {
            ListOfOrg = new Dictionary<int, string>();
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            foreach (var a in _context.KGB_OrgJed)
            {
                ListOfOrg.Add(a.SifraOj, a.NazivOj);
            }
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            Input.Lozinka = GeneratePassword(Input.Ime, Input.Prezime);
            Input = CreateKGBUser(Input, Input.Ime, Input.Prezime, Input.Naziv_Oj, Input.Email);
            if (ModelState.IsValid && Input != null)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Lozinka);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "User vec postoji");
                return Page();
            }
            return Page();
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }
        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
        private string GeneratePassword(string Ime, string Prezime)
        {
            Random rnd = new Random();
            const string chars = "#!#$%&?";

            string Password = Char.ToUpper(Ime[0]) + Prezime + rnd.Next(100, 999) + new string(Enumerable.Repeat(chars, 1)
                .Select(s => s[rnd.Next(s.Length)]).ToArray());
            return Password;
        }
        private KGB_User CreateKGBUser(KGB_User User, string Ime, string Prezime, string NazivOrgJed, string Email)
        {
            User.Ime = char.ToUpper(Ime[0]) + Ime.Substring(1);
            User.Prezime = char.ToUpper(Prezime[0]) + Prezime.Substring(1);
            User.Email = char.ToUpper(Email[0]) + Email.Substring(1);
            User.Active = true;
            User.D_Upd = DateTime.Now.ToString();
            User.Sifra_Oj = _context.KGB_OrgJed.Where(x => x.NazivOj == NazivOrgJed).FirstOrDefault().SifraOj;
            User.K_Ins = 1;
            User.K_Upd = 1;
            User.Fk_Rola = 1;
            var result = _context.KGB_Users.Where(x => x.Email == User.Email).FirstOrDefault();
            if (result != null)
            {
                return null;
            }
            _context.KGB_Users.Add(User);
            _context.SaveChanges();
            return User;
        }
    }
}
