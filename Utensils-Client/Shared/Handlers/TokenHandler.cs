using Shared.Dto.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utensils_Client.Shared.Handlers
{
    public class TokenHandler : DelegatingHandler
    {
        private CustomAuthenticationStateProvider _stateProvider;

        public TokenHandler(CustomAuthenticationStateProvider customAuthenticationStateProvider)
        {
            _stateProvider = customAuthenticationStateProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken
        ) {
            string token = await _stateProvider.GetUserToken();
            request.Headers.Add("Bearer", token);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
