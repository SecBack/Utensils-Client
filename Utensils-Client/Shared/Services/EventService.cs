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
    public class EventService
    {
        private IHttpClientFactory _httpClientFactory { get; set; }
        private HttpClient _httpClient { get; set; }

        public EventService(
            IHttpClientFactory httpClientFactory
        )
        {
            _httpClientFactory = httpClientFactory;

            _httpClient = _httpClientFactory.CreateClient("Data");
        }

        public async Task<List<EventDto>> GetEvents()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("/api/events");
            response.EnsureSuccessStatusCode();
            List<EventDto> events = await response.Content.ReadFromJsonAsync<List<EventDto>>();

            return events;
        }

        public async Task<EventDto> CreateEvent(CreateEventRequest request)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/api/events/create", request);
            response.EnsureSuccessStatusCode();
            EventDto createdEvent = await response.Content.ReadFromJsonAsync<EventDto>();

            return createdEvent;
        }
    }
}
