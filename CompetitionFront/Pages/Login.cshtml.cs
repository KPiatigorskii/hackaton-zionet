using Microsoft.AspNetCore.Authentication;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ZionetCompetition.Pages
{
	public class LoginModel : PageModel
	{
        public async Task OnGet(string redirectUri)
        {
            //var baseUri = "http://competitionfront/"
            var baseUri = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            //var completeRedirectUri = $"{baseUri}{redirectUri}";
            var completeRedirectUri = "http://competitionfront/";

            var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
                .WithRedirectUri(completeRedirectUri)
                .Build();

            await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        }
    }
}