
using Microsoft.EntityFrameworkCore;
using MsSqlAccessor.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.Extensions.Hosting;
using MsSqlAccessor.Hubs;
using MsSqlAccessor.DbControllers;
using MsSqlAccessor.Services;
using Owin;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer; 
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using Azure.Core;
using System.Security.Claims;
using Task = MsSqlAccessor.Models.Task;
using Microsoft.Extensions.DependencyInjection;
using TaskStatus = MsSqlAccessor.Models.TaskStatus;

namespace MsSqlAccessor
{
	public class Program
	{
		public static void Main(string[] args)
		{
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddJsonOptions(options => {
				options.JsonSerializerOptions.PropertyNamingPolicy = null;
			});


            string domain = $"https://{builder.Configuration["Auth0:Domain"]}/";
            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = domain;
                    options.Audience = builder.Configuration["Auth0:ClientId"];
                    // If the access token does not have a `sub` claim, `User.Identity.Name` will be `null`. Map it to a different claim by setting the NameClaimType below.
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = ClaimTypes.NameIdentifier
                    };
/*                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            //var accessToken = context.Request.Query["access_token"];
                            var accessToken = context.Request.Cookies["auth_token"];

                            // If the request is for our hub...
                            var path = context.HttpContext.Request.Path;
                            *//*                        *//*                        if (!string.IsNullOrEmpty(accessToken) &&
                                                                                (path.StartsWithSegments("/hubs/chat")))*//*
                                                    {
                                                        // Read the token out of the query string*//*
                            context.Token = accessToken;

                            return System.Threading.Tasks.Task.CompletedTask;
                        }*/
                   // };
                });



            builder.Services.AddSignalR(hubOptions =>
            {
                hubOptions.EnableDetailedErrors = true;
                hubOptions.KeepAliveInterval = System.TimeSpan.FromMinutes(1);
            });
            builder.Services.AddHttpContextAccessor();

            //builder.Services.AddCors();

            builder.Services.AddDbContext<CompetitionBdTestContext>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();

            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type=ReferenceType.SecurityScheme,
                                    Id="Bearer"
                                }
                            },
                            new string[]{}
                        }
                });
            });

            //builder.Services.AddTransient<IUserRole, MockUserRole>();
            //builder.Services.AddTransient<EventsDbController>();
            builder.Services.AddTransient<GenDbController<Event, EventDTO>>();
            builder.Services.AddTransient<GenDbController<EventManager, EventManagerDTO>>();
            builder.Services.AddTransient<GenDbController<EventParticipantTeam, EventParticipantTeamDTO>>();
            builder.Services.AddTransient<GenDbController<EventTask, EventTaskDTO>>();
            builder.Services.AddTransient<GenDbController<EventTaskEvaluateUser, EventTaskEvaluateUserDTO>>();
            builder.Services.AddTransient<GenDbController<Role, RoleDTO>>();
            builder.Services.AddTransient<GenDbController<Task, TaskDTO>>();
            builder.Services.AddTransient<GenDbController<TaskCategory, TaskCategoryDTO>>();
            builder.Services.AddTransient<GenDbController<TaskParticipant, TaskParticipantDTO>>();
            builder.Services.AddTransient<GenDbController<Team, TeamDTO>>();
            builder.Services.AddTransient<GenDbController<TeamTask, TeamTaskDTO>>();
            builder.Services.AddTransient<GenDbController<User, UserDTO>>();
            builder.Services.AddTransient<GenDbController<EventStatus, EventStatusDTO>>();
            builder.Services.AddTransient<GenDbController<TaskStatus, TaskStatusDTO>>();
            builder.Services.AddTransient<AuthUserDbController>();




            var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();


			app.UseRouting();
            //app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseAuthentication();
            app.UseAuthorization();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: default,
					pattern: "{controller=Home}/{action=Index}/{id?}");
				//endpoints.MapHub<UserHub>("/users");
			});

            //app.MapHub<UserHub>("/users");
            //app.MapHub<MssqlHubOld<User>>("/users");
            //app.MapHub<EventParticipantTeamHub>("/eventparticipantteams");
            //app.MapHub<EventHub>("/events");
            //app.MapHub<MsSQLHub<Event, EventDTO>>("/events");
            app.MapHub<EventHub<Event, EventDTO>>("/events");
            app.MapHub<EventManagerHub<EventManager, EventManagerDTO>>("/EventManagers");
			app.MapHub<EventParticipantTeamHub<EventParticipantTeam, EventParticipantTeamDTO>>("/eventparticipantteams");
			app.MapHub<EventTaskHub<EventTask, EventTaskDTO>>("/EventTasks");
			app.MapHub<EventTaskEvaluateUserHub<EventTaskEvaluateUser, EventTaskEvaluateUserDTO>>("/EventTaskEvaluateUsers");
			app.MapHub<RoleHub<Role, RoleDTO>>("/Roles");
			app.MapHub<TaskHub<Task, TaskDTO>>("/TaskModels");
			app.MapHub<TaskCategoryHub<TaskCategory, TaskCategoryDTO>>("/TaskCategorys");
			app.MapHub<TaskParticipantHub<TaskParticipant, TaskParticipantDTO>>("/TaskParticipants");
			app.MapHub<TeamHub<Team, TeamDTO>>("/Teams");
			app.MapHub<TeamTaskHub<TeamTask, TeamTaskDTO>>("/TeamTasks");
			app.MapHub<UserHub<User, UserDTO>>("/Users");
            app.MapHub<EventStatusHub<EventStatus, EventStatusDTO>>("/EventStatuss");
            app.MapHub<TaskStatusHub<TaskStatus, TaskStatusDTO>>("/TaskStatuss");

            app.MapControllers();
			app.Run();
		}
		public void Configuration(IAppBuilder app)
		{
			// Any connection or hub wire up and configuration should go here
			GlobalHost.HubPipeline.AddModule(new ErrorHandlingPipelineModule());
			app.MapSignalR();
		}
	}
}