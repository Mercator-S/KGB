using KGB_Dev_.Interfaces;
using KGB_Models.KGB_Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;

namespace KGB_Dev_.Shared
{

    public partial class NavMenu
    {
        [Inject]
        public IDataRetrivingServices IGetServices { get; set; } = default!;
        [Parameter]
        public int Role { get; set; }
        protected override async Task OnInitializedAsync()
        {
            KGB_User User = IGetServices.GetCurrentUser().Result;
            Role = User.Fk_Rola;
        }

    }
}

