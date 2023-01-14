using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;

namespace ZionetCompetition.Services
{
    public class TokenService
    {
        private string _token;
        private readonly ErrorService _errorService;
        private IHttpContextAccessor _httpContextAccessor;

        private AuthenticationStateProvider _authenticationStateProvider;

        public TokenService(ErrorService errorService, IHttpContextAccessor httpContextAccessor, AuthenticationStateProvider authenticationStateProvider)
        {
            _errorService = errorService;
            _httpContextAccessor = httpContextAccessor;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<string> GetToken()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            _token = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(e => e.Type == "jwt_token")?.Value;
            if (_token == null)
            {
                _errorService.Redirect(Errors.Errors.NotLogin);
            }

            return _token;

        }
    }
}
