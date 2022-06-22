using KGB_Dev_.Data.KGB_Model;
using KGB_Dev_.DataRetrieving;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace KGB_Dev_.Shared
{

    public partial class NavMenu
    {
        [Inject]
        public IKgbServices IServices { get; set; } = default!;
        [Parameter]
        public int Role { get; set; }
        protected override async Task OnInitializedAsync()
        {
            var a = IServices.GetCurrentUser().Result;
            Role = a.Result.Fk_Rola;
        }

    }
}

