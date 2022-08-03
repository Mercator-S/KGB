using KGB_Dev_.Interfaces;
using Microsoft.AspNetCore.Components;

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
            var a = IGetServices.GetCurrentUser().Result;
            Role = a.Fk_Rola;
        }

    }
}

