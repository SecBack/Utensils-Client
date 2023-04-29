using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utensils_Client.Shared.Services
{
    public class GroupSerivce
    {
        private IHttpClientFactory httpClientFactory { get; set; }
        private HttpClient httpClient { get; set; }

        public GroupService(
            IHttpClientFactory httpClientFactory
        ) {
            this.httpClientFactory = httpClientFactory;

            httpClient = this.httpClientFactory.CreateClient("Data");
        }

    }
}
