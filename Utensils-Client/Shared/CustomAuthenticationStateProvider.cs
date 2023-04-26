using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Shared.DtoModels;
using System.Security.Claims;

namespace Utensils_Client.Shared
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
        public async Task Login(UserDto userDto)
        {
            //Again, this is what I'm doing, but you could do/store/save anything as part of this process
            string userDtoJson = JsonConvert.SerializeObject(userDto);
            await SecureStorage.SetAsync("userDto", userDtoJson);

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
            SecureStorage.Remove("userDto");
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
                var userString = await SecureStorage.GetAsync("userDto");
                UserDto user = JsonConvert.DeserializeObject<UserDto>(userString);
                if (user != null)
                {
                    var claims = new[] {
                        new Claim(ClaimTypes.Name, user.Name),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.MobilePhone)
                    };
                    var identity = new ClaimsIdentity(claims, "Custom authentication");
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