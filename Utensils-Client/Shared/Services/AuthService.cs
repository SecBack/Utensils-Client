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

        public async Task Logout()
        {
            await _customAuthenticationStateProvider.Logout();
            _navigationManager.NavigateTo("/login");
        }

        public async Task<UserDto> GetCurrentUser()
        {
            AuthenticationState authState = await _customAuthenticationStateProvider.GetAuthenticationStateAsync();
            var id = authState.User.Claims.First(c => c.Type.Contains("nameidentifier")).Value;
            //var userName = authState.User.Claims.First(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
            //var email = authState.User.Claims.First(c => c.Type.Contains("email")).Value;

            HttpResponseMessage response = await _httpClient.GetAsync($"/api/users/{id}/details");
            UserDto user = await response.Content.ReadFromJsonAsync<UserDto>();

            return user;
        }

        public async Task<Guid> GetUserId()
        {
            AuthenticationState authState = await _customAuthenticationStateProvider.GetAuthenticationStateAsync();
            var id = authState.User.Claims.First(c => c.Type.Contains("nameidentifier")).Value;

            return Guid.Parse(id);
        }
    }
}
