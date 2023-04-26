using Microsoft.AspNetCore.Components;
using Shared.DtoModels;
using System.Net.Http;
using System.Net.Http.Json;
using Utensils_Client.Shared;
using Utensils_Client.Shared.Services;

namespace Utensils_Client.Pages.Auth
{
    public class RegisterPageLogic : ComponentBase
    {
        [Inject] private AuthService authService { get; set; }
        protected RegisterDto RegisterModel { get; set; } = new RegisterDto();

        protected async void OnRegister()
        {
            // using the auth service register the user
            await authService.Register(RegisterModel);
        }
    }
}
