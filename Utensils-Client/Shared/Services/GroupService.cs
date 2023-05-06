using Shared.Dto.Models;
using Shared.Requests;
using System.Net.Http.Json;

namespace Utensils_Client.Shared.Services
{
    public class GroupService
    {
        private IHttpClientFactory httpClientFactory { get; set; }
        private HttpClient httpClient { get; set; }

        public GroupService(
            IHttpClientFactory httpClientFactory
        ) {
            this.httpClientFactory = httpClientFactory;

            httpClient = this.httpClientFactory.CreateClient("Data");
        }

        public async Task<List<GroupDto>> GetGroups()
        {
            HttpResponseMessage response = await httpClient.GetAsync("/api/groups");
            List<GroupDto> groups = await response.Content.ReadFromJsonAsync<List<GroupDto>>();

            return groups;
        }

        public async Task<GroupDto> GetGroupMembers(Guid groupId)
        {
            HttpResponseMessage response = await httpClient.GetAsync($"/api/groups/{groupId}");
            response.EnsureSuccessStatusCode();
            GroupDto group = await response.Content.ReadFromJsonAsync<GroupDto>();

            return group;
        }   

        public async Task<GroupDto> CreateGroup(CreateGroupRequest createGroupRequest)
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("/api/groups", createGroupRequest);
            response.EnsureSuccessStatusCode();
            GroupDto group = await response.Content.ReadFromJsonAsync<GroupDto>();

            return group;
        }

        public async Task<GroupDto> UpdateGroup(UpdateGroupRequest updateGroupRequest)
        {
            HttpResponseMessage response = await httpClient.PutAsJsonAsync("/api/groups", updateGroupRequest);
            response.EnsureSuccessStatusCode();
            GroupDto group = await response.Content.ReadFromJsonAsync<GroupDto>();

            return group;
        }
    }
}
