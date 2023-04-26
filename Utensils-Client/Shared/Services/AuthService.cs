using Microsoft.AspNetCore.Components;
using Shared.DtoModels;
using System.Net.Http.Json;
using Utensils_Client.Shared;

namespace Utensils_Client.Shared.Services
{
    public class AuthService
    {
        private NavigationManager navigationManager { get; set; }
        private IHttpClientFactory httpClientFactory { get; set; }
        private HttpClient httpClient { get; set; }
        private CustomAuthenticationStateProvider customAuthenticationStateProvider { get; set; }

        public AuthService(
            IHttpClientFactory httpClientFactory,
            CustomAuthenticationStateProvider customAuthenticationStateProvider,
            NavigationManager navigationManager
        ) {
            this.httpClientFactory = httpClientFactory;
            this.customAuthenticationStateProvider = customAuthenticationStateProvider;
            this.navigationManager = navigationManager;

            httpClient = this.httpClientFactory.CreateClient("Auth");
        }

        public async Task Register(RegisterDto registerModel)
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("/register", registerModel);
            if (response.IsSuccessStatusCode)
            {
                UserDto userDto = await response.Content.ReadFromJsonAsync<UserDto>();
                await customAuthenticationStateProvider.Login(userDto);

                navigationManager.NavigateTo("/calender");
            }
        }

        public async Task Login(LoginDto loginModel)
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("/login", loginModel);
            if (response.IsSuccessStatusCode)
            {
                UserDto userDto = await response.Content.ReadFromJsonAsync<UserDto>();
                await customAuthenticationStateProvider.Login(userDto);

                navigationManager.NavigateTo("/calender");
            }
        }
    }
}
