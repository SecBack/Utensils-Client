using Newtonsoft.Json;
using Shared.Dto.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
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
            //string token = await GetUserToken();
            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return await base.SendAsync(request, cancellationToken);
        }

        public async Task<string> GetUserToken()
        {
            var userJson = await SecureStorage.GetAsync("user");
            if (userJson == null)
            {
                return null;
            }

            AuthModel user = JsonConvert.DeserializeObject<AuthModel>(userJson);

            return user.Token;
        }
    }
}
