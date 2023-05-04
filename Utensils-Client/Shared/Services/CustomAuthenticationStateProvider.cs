using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Shared.Dto.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Utensils_Client.Shared.Services
{
    /// <summary>
    /// This is our custom implementation of an authentication state provider.  The primary purpose of this is 
    /// to implement the overriden method, which is used internally by .NET to determine the current user identity.
    /// For clarity, and ease of use I put the code here that is used to actually trigger login & logout.
    /// </summary>
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        public CustomAuthenticationStateProvider()
        {
        }


        /// <summary>
        /// This method should be called upon a successful user login, and it will store the user's JWT token in SecureStorage.
        /// Upon saving this it will also notify .NET that the authentication state has changed which will enable authenticated views
        /// </summary>
        /// <param name="token">Our JWT to store</param>
        /// <returns></returns>
        public async Task Login(AuthModel auth)
        {
            //Again, this is what I'm doing, but you could do/store/save anything as part of this process
            await SecureStorage.SetAsync("jwtToken", auth.Token);

            //Providing the current identity ifnormation
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        /// <summary>
        /// This method should be called to log-off the user from the application, which simply removed the stored token and then
        /// notifies of the change
        /// </summary>
        /// <returns></returns>
        public async Task Logout()
        {
            SecureStorage.Remove("jwtToken");
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        /// <summary>
        /// This is the key method that is called by .NET to accomplish our goal.  It is the method that is queried to get the current 
        /// AuthenticationState for the user.  In our base, if we have a token in secure storage, we are logged in, but we could easily
        /// do much more than this. 
        /// </summary>
        /// <returns></returns>
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
                    string phone = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == "phone")?.Value;

                    var identity = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, userId),
                        new Claim(ClaimTypes.Name, userName),
                        new Claim(ClaimTypes.Email, email),
                        new Claim(ClaimTypes.MobilePhone, phone),
                    }, "Custom authentication");

                    return new AuthenticationState(new ClaimsPrincipal(identity));
                }
            }
            catch (Exception ex)
            {
                //This should be more properly handled
                Console.WriteLine("Request failed:" + ex.ToString());
            }

            return new AuthenticationState(new ClaimsPrincipal());
        }
    }
}