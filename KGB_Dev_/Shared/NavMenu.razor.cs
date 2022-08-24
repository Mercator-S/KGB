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
        public KGB_User User { get; set; }
        protected override async Task OnInitializedAsync()
        {
             User = IGetServices.GetCurrentUser().Result;
        }

    }
}

