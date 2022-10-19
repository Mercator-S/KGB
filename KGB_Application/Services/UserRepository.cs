using KGB_Application.Interfaces;
using KGB_Models.KGB_Model;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGB_Application.Services
{
    public class UserRepository: IUserRepository
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly UserManager<KGB_User> _UserManager;

        public UserRepository(AuthenticationStateProvider authenticationStateProvider, UserManager<KGB_User> userManager)
        {
            _UserManager = userManager;
            _authenticationStateProvider = authenticationStateProvider;
        }
        public async Task<KGB_User> GetCurrentUser()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            return await Task.FromResult(_UserManager.GetUserAsync(authState.User).Result);
        }
    }
}
