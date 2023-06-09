﻿using Microsoft.AspNetCore.Components;
using Shared.Dto.Auth;
using Utensils_Client.Shared.Services;

namespace Utensils_Client.Pages
{
    public class LoginPageLogic : ComponentBase
    {
        [Inject] private AuthService AuthService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        protected LoginDto LoginDto { get; set; } = new LoginDto();

        protected void NavigateToRegister()
        {
            NavigationManager.NavigateTo("/register");
        }

        protected async Task OnLogin()
        {
            // use the auth service to login the user
            await AuthService.Login(LoginDto);
        }
    }
}
