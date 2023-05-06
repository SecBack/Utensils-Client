using Shared.Dto.Models;
using Shared.Requests;
using System.Net.Http.Json;

namespace Utensils_Client.Shared.Services
{
    public class GroupService
    {
        private IHttpClientFactory _httpClientFactory { get; set; }
        private HttpClient _httpClient { get; set; }

        public GroupService(
            IHttpClientFactory httpClientFactory
        ) {
            _httpClientFactory = httpClientFactory;

            _httpClient = _httpClientFactory.CreateClient("Data");
        }

        public async Task<List<GroupDto>> GetGroups()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("/api/groups");
            response.EnsureSuccessStatusCode();
            List<GroupDto> groups = await response.Content.ReadFromJsonAsync<List<GroupDto>>();

            return groups;
        }

        public async Task<GroupDto> GetGroupMembers(Guid groupId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"/api/groups/{groupId}");
            response.EnsureSuccessStatusCode();
            GroupDto group = await response.Content.ReadFromJsonAsync<GroupDto>();

            return group;
        }   

        public async Task<GroupDto> CreateGroup(CreateGroupRequest createGroupRequest)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/api/groups", createGroupRequest);
            response.EnsureSuccessStatusCode();
            GroupDto group = await response.Content.ReadFromJsonAsync<GroupDto>();

            return group;
        }

        public async Task<GroupDto> UpdateGroup(UpdateGroupRequest updateGroupRequest)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync("/api/groups", updateGroupRequest);
            response.EnsureSuccessStatusCode();
            GroupDto group = await response.Content.ReadFromJsonAsync<GroupDto>();

            return group;
        }
    }
}
