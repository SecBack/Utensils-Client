using Shared.Dto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Utensils_Client.Shared.Services
{
    public class EventService
    {
        private IHttpClientFactory httpClientFactory { get; set; }
        private HttpClient httpClient { get; set; }

        public EventService(
            IHttpClientFactory httpClientFactory
        )
        {
            this.httpClientFactory = httpClientFactory;

            httpClient = this.httpClientFactory.CreateClient("Data");
        }

        public async Task<List<EventDto>> GetEvents()
        {
            HttpResponseMessage response = await httpClient.GetAsync("/api/events");
            response.EnsureSuccessStatusCode();
            List<EventDto> events = await response.Content.ReadFromJsonAsync<List<EventDto>>();

            return events;
        }
    }
}
