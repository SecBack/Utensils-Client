using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Shared.Dto.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Utensils_Client.Shared.Services
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        public async Task Login(AuthModel auth)
        {
            await SecureStorage.SetAsync("jwtToken", auth.Token);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task Logout()
        {
            SecureStorage.Remove("jwtToken");
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                string jwtToken = await SecureStorage.GetAsync("jwtToken");
                if (jwtToken != null)
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var jwtSecurityToken = tokenHandler.ReadJwtToken(jwtToken);
                    string userId = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;
                    string userName = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == "unique_name")?.Value;
                    string email = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
                    //string phone = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == "phone")?.Value;

                    var identity = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, userId),
                        new Claim(ClaimTypes.Name, userName),
                        new Claim(ClaimTypes.Email, email)
                        //new Claim(ClaimTypes.MobilePhone, phone),
                    }, "Custom authentication");

                    return new AuthenticationState(new ClaimsPrincipal(identity));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Request failed:" + ex.ToString());
            }

            return new AuthenticationState(new ClaimsPrincipal());
        }
    }
}