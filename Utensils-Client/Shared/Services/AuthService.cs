using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Shared.Dto.Auth;
using Shared.Dto.Models;
using System.Net.Http.Json;

namespace Utensils_Client.Shared.Services
{
    public class AuthService
    {
        private NavigationManager _navigationManager { get; set; }
        private IHttpClientFactory _httpClientFactory { get; set; }
        private HttpClient _httpClient { get; set; }
        private CustomAuthenticationStateProvider _customAuthenticationStateProvider { get; set; }

        public AuthService(
            IHttpClientFactory httpClientFactory,
            CustomAuthenticationStateProvider customAuthenticationStateProvider,
            NavigationManager navigationManager
        ) {
            _httpClientFactory = httpClientFactory;
            _customAuthenticationStateProvider = customAuthenticationStateProvider;
            _navigationManager = navigationManager;

            _httpClient = _httpClientFactory.CreateClient("Auth");
        }

        public async Task Register(RegisterDto registerDto)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/api/register", registerDto);
            if (response.IsSuccessStatusCode)
            {
                AuthModel token = await response.Content.ReadFromJsonAsync<AuthModel>();
                await _customAuthenticationStateProvider.Login(token);

                _navigationManager.NavigateTo("/calender");
            }
        }

        public async Task Login(LoginDto loginDto)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/api/login", loginDto);
            if (response.IsSuccessStatusCode)
            {
                AuthModel token = await response.Content.ReadFromJsonAsync<AuthModel>();
                await _customAuthenticationStateProvider.Login(token);

                _navigationManager.NavigateTo("/calender");
            }
        }

        public async Task<UserDto> GetCurrentUser()
        {
            AuthenticationState authState = await _customAuthenticationStateProvider.GetAuthenticationStateAsync();
            string? userName = authState.User.Identity.Name;

            if (userName == null)
            {
                return null;
            }

            HttpResponseMessage response = await _httpClient.GetAsync($"/api/users/{userName}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<UserDto>();
        }
    }
}
