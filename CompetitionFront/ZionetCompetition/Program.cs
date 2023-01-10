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

//builder.Services.AddTransient<UserController>();
//builder.Services.AddTransient<EventController>();
//builder.Services.AddTransient<UserEventTeamController>();

builder.Services.AddTransient<GenClientController<EventModel>>();
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
            OnTokenValidated = (context) =>
            {
                var token = context.SecurityToken.RawHeader+ "." + 
                context.SecurityToken.RawPayload + "." + context.SecurityToken.RawSignature;
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

                return Task.CompletedTask;
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
