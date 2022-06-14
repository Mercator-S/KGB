// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable
using System.Text;
using KGB_Dev_.Data;
using KGB_Dev_.Data.KGB_Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace KGB_Dev_.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<KGB_User> _signInManager;
        private readonly UserManager<KGB_User> _userManager;
        private readonly IUserStore<KGB_User> _userStore;
        private readonly IUserEmailStore<KGB_User> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly ApplicationDbContext _context;

        public RegisterModel(
            UserManager<KGB_User> userManager,
            IUserStore<KGB_User> userStore,
            SignInManager<KGB_User> signInManager,
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
        public Dictionary<int, string> ListOfRola { get; set; }
        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public async Task OnGetAsync(string returnUrl = null)
        {
            ListOfOrg = new Dictionary<int, string>();
            ListOfRola = new Dictionary<int, string>();
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            foreach (var a in _context.KGB_OrgJed)
            {
                ListOfOrg.Add(a.SifraOj, a.NazivOj);
            }
            foreach (var role in _context.KGB_Role)
            {
                ListOfRola.Add(role.Sifra_Role, role.Naziv_Role);

            }
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/Home");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            Input = CreateKGBUser(Input.Ime, Input.Prezime, Input.Naziv_Oj, Input.Email, Input.Naziv_Role);
            if (ModelState.IsValid && Input != null)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(Input, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(Input, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(Input, Input.Lozinka);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(Input);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(Input);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(Input, isPersistent: false);
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

        private KGB_User CreateUser()
        {
            try
            {
                return Activator.CreateInstance<KGB_User>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(KGB_User)}'. " +
                    $"Ensure that '{nameof(KGB_User)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }
        private IUserEmailStore<KGB_User> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<KGB_User>)_userStore;
        }
        private string GeneratePassword(string Ime, string Prezime)
        {
            Random rnd = new Random();
            const string chars = "#!#$%&?";

            string Password = Char.ToUpper(Ime[0]) + Prezime + rnd.Next(100, 999) + new string(Enumerable.Repeat(chars, 1)
                .Select(s => s[rnd.Next(s.Length)]).ToArray());
            return Password;
        }
        private KGB_User CreateKGBUser(string Ime, string Prezime, string NazivOrgJed, string Email, string Rola)
        {
            KGB_User User = new KGB_User();
            User.Ime = char.ToUpper(Ime[0]) + Ime.Substring(1);
            User.Prezime = char.ToUpper(Prezime[0]) + Prezime.Substring(1);
            User.Lozinka = GeneratePassword(Input.Ime, Input.Prezime);
            User.Email = char.ToUpper(Email[0]) + Email.Substring(1);
            User.Active = true;
            User.D_Upd = DateTime.Now.ToString();
            User.Naziv_Oj = NazivOrgJed;
            User.Sifra_Oj = _context.KGB_OrgJed.Where(x => x.NazivOj == NazivOrgJed).FirstOrDefault().SifraOj;
            User.K_Ins = 1;
            User.K_Upd = 1;
            User.Fk_Rola = _context.KGB_Role.Where(x => x.Naziv_Role == Rola).FirstOrDefault().Sifra_Role;
            User.Naziv_Role = Rola;
            var result = _context.KGB_Users.Where(x => x.Email == User.Email).FirstOrDefault();
            if (result != null)
            {
                return null;
            }
            //_context.KGB_Users.Add(User);
            //_context.SaveChanges();
            return User;
        }
    }
}
