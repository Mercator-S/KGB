using KGB_Dev_.Data.KGB_Model;
using Microsoft.AspNetCore.Identity;

namespace KGB_Dev_.Shared
{

    public partial class NavMenu
    {
        //private readonly UserManager<KGB_User> _userManager;
        //public int Fk_Rola { get; set; }

        //protected NavMenu(UserManager<KGB_User> userManager)
        //{
        //    this._userManager = userManager;
        //}
        public async Task OnGetAsync(string returnUrl = null)
        {
            var a = _userManager.Users;
        }

    }

}

