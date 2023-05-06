using Shared.Dto.Models;
using System.Net.Http.Json;

namespace Utensils_Client.Shared.Services
{
    public class UserService
    {
        private AuthService _authService { get; set; }
        private IHttpClientFactory _httpClientFactory { get; set; }
        private HttpClient _httpClient { get; set; }

        public UserService(
            AuthService authService,
            IHttpClientFactory httpClientFactory
        ) {
            _authService = authService;

            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("Data");
        }

        public async Task<UserDto> GetUserDetails()
        {
            Guid userId = await _authService.GetUserId();

            HttpResponseMessage response = await _httpClient.GetAsync($"/api/users/{userId}/details");
            response.EnsureSuccessStatusCode();
            UserDto userWithDetails = await response.Content.ReadFromJsonAsync<UserDto>();

            return userWithDetails;
        }
    }
}
