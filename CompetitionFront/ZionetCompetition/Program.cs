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
using TaskStatus = ZionetCompetition.Models.TaskStatus;
using NuGet.Protocol.Plugins;

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
builder.Services.AddTransient<FlagService>();
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
builder.Services.AddTransient<GenClientController<TeamTask>>();
builder.Services.AddTransient<GenClientController<User>>();
builder.Services.AddTransient<GenClientController<EventStatus>>();
builder.Services.AddTransient<GenClientController<TaskStatus>>();
builder.Services.AddTransient<AuthClientController<User>>();


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

                var AuthenticationController = context.HttpContext.RequestServices.GetRequiredService<AuthClientController<User>>();
                AuthenticationController.ConfigureHub(token);
                await AuthenticationController.StartConnection();
                await AuthenticationController.Get(email);
                var existedUser = AuthenticationController.message;
                if (existedUser.Id == 0)
                {
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
                    await AuthenticationController.Register(user);
                }
                else
                {
					var EventParticipantTeamController = context.HttpContext.RequestServices.GetRequiredService<GenClientController<EventParticipantTeam>>();
					Dictionary<string, object> currentEventIdFilter = new Dictionary<string, object>() { { "ParticipantId", existedUser.Id }, { "IsActive", true } };
                    await EventParticipantTeamController.ConfigureHub(token);
                    await EventParticipantTeamController.StartConnection();
                    await EventParticipantTeamController.GetAllWithConditions(currentEventIdFilter);

					var additionalClaims = new List<Claim>
							{
								new Claim("currentEventId", "0"),
								new Claim("currentEventName", ""),
								new Claim("isLeader", "false"),
								new Claim("isActive", "false"),
								new Claim("currentTeamId", "0"),
								new Claim("currentTeamName", ""),
								new Claim("isApplied", "false"),
							};


					if (EventParticipantTeamController.messages.Count() > 0) //TODO: ANY
                    {
						var currentEventId = EventParticipantTeamController.messages.First().EventId;
						var isLeader = EventParticipantTeamController.messages.First().IsLeader;
                        var isActive = EventParticipantTeamController.messages.First().IsActive;
                        var currentTeamId = EventParticipantTeamController.messages.First().TeamId;
                        var isApplied = EventParticipantTeamController.messages.First().IsApplied;

                        var EventController = context.HttpContext.RequestServices.GetRequiredService<GenClientController<Event>>();
                        await EventController.ConfigureHub(token);
                        await EventController.StartConnection();
                        await EventController.GetOneWithConditions( new Dictionary<string, object>() { { "Id", currentEventId } });
                        var currentEventName = EventController.message.Title;

						additionalClaims = new List<Claim>
							{
								new Claim("currentEventId", currentEventId.ToString()),
								new Claim("currentEventName", currentEventName.ToString()),
								new Claim("isLeader", isLeader.ToString()),
								new Claim("isActive", isActive.ToString()),
								new Claim("currentTeamId", currentTeamId.ToString()),
								new Claim("isApplied", isApplied.ToString()),
							};

						if (currentTeamId is not null)
                        {
							var TeamController = context.HttpContext.RequestServices.GetRequiredService<GenClientController<Team>>();
							await TeamController.ConfigureHub(token);
							await TeamController.StartConnection();
							await TeamController.GetOneWithConditions(new Dictionary<string, object>() { { "Id", currentTeamId } });
							var currentTeamName = TeamController.message.Title;
                            //additionalClaims.Add(new Claim("currentTeamId", currentTeamId.ToString()));
                            additionalClaims.Add(new Claim("currentTeamName", currentTeamName.ToString()));
						}
					}
					appIdentity = new ClaimsIdentity(additionalClaims, CookieAuthenticationDefaults.AuthenticationScheme);
					context.Principal.AddIdentity(appIdentity);
				}
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
