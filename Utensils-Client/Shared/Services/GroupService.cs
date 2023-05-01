using Microsoft.AspNetCore.Components;
using Shared.Dto.Auth;
using Shared.Dto.Models;
using Shared.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

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

            var hej = "hej";

            List<GroupDto> groups = await response.Content.ReadFromJsonAsync<List<GroupDto>>();

            return groups;
        }

        public async Task<GroupDto> CreateGroup(CreateGroupRequest createGroupRequest)
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("/api/groups", createGroupRequest);
            GroupDto group = await response.Content.ReadFromJsonAsync<GroupDto>();

            return group;
        }
    }
}
