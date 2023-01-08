namespace ZionetCompetition.Services
{
    public class TokenService
    {

        public string GetToken(IHttpContextAccessor httpContextAccessor) 
        { 
            return httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(e => e.Type == "jwt_token").Value;
        }
    }
}
