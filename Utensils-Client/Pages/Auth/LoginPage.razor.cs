using Microsoft.AspNetCore.Components;
using Shared.DtoModels;

namespace Utensils_Client.Pages
{
    public class LoginPageLogic : ComponentBase
    {
        [Inject] private NavigationManager NavigationManager { get; set; }

        public LoginModel LoginModel { get; set; } = new LoginModel();

        protected void OnRegister()
        {
            NavigationManager.NavigateTo("/register");
        }
    }
}
