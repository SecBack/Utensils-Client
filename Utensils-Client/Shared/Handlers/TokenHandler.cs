using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utensils_Client.Shared.Handlers
{
    public class TokenHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken
        ) {
            string token = await SecureStorage.Default.GetAsync("authToken");
            request.Headers.Add("Bearer", token);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
