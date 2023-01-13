using Auth0.AspNetCore.Authentication;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ZionetCompetition.Controllers;
using ZionetCompetition.Data;
using Microsoft.JSInterop;
using Microsoft.DotNet.Scaffolding.Shared.ProjectModel;
using BlazorBootstrap;
using ZionetCompetition.Services;
using ZionetCompetition.Models;
using Autofac.Core;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddControllers();
builder.Services.AddSession();

builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<TokenService>();
builder.Services.AddTransient<ErrorService>();
builder.Services.AddTransient<TwitterService>();
builder.Services.AddTransient<TwitterEngineService>();

//builder.Services.AddTransient<UserController>();
//builder.Services.AddTransient<EventController>();
//builder.Services.AddTransient<UserEventTeamController>();

builder.Services.AddTransient<GenClientController<Event>>();
builder.Services.AddTransient<GenClientController<EventManager>>();
builder.Services.AddTransient<GenClientController<EventParticipantTeam>>();
builder.Services.AddTransient<GenClientController<EventTask>>();
builder.Services.AddTransient<GenClientController<EventTaskEvaluateUser>>();
builder.Services.AddTransient<GenClientController<Role>>();
builder.Services.AddTransient<GenClientController<TaskCategory>>();
builder.Services.AddTransient<GenClientController<TaskModel>>();
builder.Services.AddTransient<GenClientController<TaskParticipant>>();
builder.Services.AddTransient<GenClientController<Team>>();
builder.Services.AddTransient<GenClientController<TeamParticipant>>();
builder.Services.AddTransient<GenClientController<TeamTask>>();
builder.Services.AddTransient<GenClientController<User>>();



builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrapProviders()
    .AddFontAwesomeIcons();
builder.Services.AddBlazorBootstrap();
builder.Services.AddBlazoredLocalStorage();   // local storage
builder.Services.AddBlazoredLocalStorage(config => config.JsonSerializerOptions.WriteIndented = true);  // local storage
builder.Services.AddSession();
builder.Services.AddDbContext<ZionetCompetitionContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ZionetCompetitionContext") ?? throw new InvalidOperationException("Connection string 'ZionetCompetitionContext' not found.")));
builder.Services.AddServerSideBlazor();


builder.Services
    .AddAuth0WebAppAuthentication(options =>
    {
        options.Domain = builder.Configuration["Auth0:Domain"];
        options.ClientId = builder.Configuration["Auth0:ClientId"];
        options.ClientSecret = builder.Configuration["Auth0:ClientSecret"];
        options.OpenIdConnectEvents = new OpenIdConnectEvents
        {
            OnTokenValidated = async (context) =>
            {


                var token = context.SecurityToken.RawHeader+ "." + 
                context.SecurityToken.RawPayload + "." + context.SecurityToken.RawSignature;
                var email = context.Principal.Claims.FirstOrDefault(e => e.Type == "http://zionet-api/user/claims/email").Value;


                var claims = new List<Claim>
                    {
                        new Claim("jwt_token", token)
                    };
                var appIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                context.Principal.AddIdentity(appIdentity);
                context.Response.Cookies.Append("auth_token", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                });


                var UserController = context.HttpContext.RequestServices.GetRequiredService<GenClientController<User>>();
                UserController.ConfigureHub(token);

                await UserController.StartConnection();
                //var id = await UserController.Get(email);
/*                if (!id) 
                {*/
                    var user = new User
                    {

                        Email = email,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        RoleId = 1,
                        Login = context.Principal.Claims.FirstOrDefault(e => e.Type == "name").Value,
                        CreateUserId = 1,
                        UpdateUserId = 1,
                        StatusId = 1,
                        FirstName = context.Principal.Claims.FirstOrDefault(e => e.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname").Value,
                        LastName = context.Principal.Claims.FirstOrDefault(e => e.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname").Value,

                    };
                    await UserController.Create(user);
             //   } 



            },

            OnTicketReceived = notification =>
            {
                Console.WriteLine("Authentication ticket received.");
                return Task.FromResult(true);
            },

            OnAuthorizationCodeReceived = notification =>
            {
                Console.WriteLine("Authorization code received");
                return Task.FromResult(true);
            },
            OnUserInformationReceived = notification =>
            {
                Console.WriteLine("Token validation received");
                return Task.FromResult(true);
            }
        };

    }).WithAccessToken(options =>
    {
        options.Audience = builder.Configuration["Auth0:Audience"];
        options.UseRefreshTokens = true;
    });

builder.Services.AddAuth0AuthenticationClient(config =>
{
    config.Domain = builder.Configuration["Auth0:Authority"];
    config.ClientId = builder.Configuration["Auth0:ClientId"];
    config.ClientSecret = builder.Configuration["Auth0:ClientSecret"];
});

builder.Services.AddAuth0ManagementClient().AddManagementAccessToken();

builder.Services.AddHttpClient();

builder.Services.AddHttpContextAccessor();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
