
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
using Microsoft.AspNetCore.Authorization;
using MsSqlAccessor.Helpers;
using NLog;
using MsSqlAccessor.Interfaces;
using MsSqlAccessor.Managers;

namespace MsSqlAccessor
{
	public class Program
	{
		public static void Main(string[] args)
		{
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("admin", policy => policy.Requirements.Add(new AuthorizationRequirementPolicy("admin")));
                options.AddPolicy("manager", policy => policy.Requirements.Add(new AuthorizationRequirementPolicy("manager")));
                options.AddPolicy("participant", policy => policy.Requirements.Add(new AuthorizationRequirementPolicy("participant")));
});

            builder.Services.AddSingleton<IAuthorizationHandler, PolicyAuthorizationHandler>();

            // Add services to the container.

			LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
			builder.Services.ConfigureLoggerService();


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
                    options.Audience = builder.Configuration["Auth0:Audience"];
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
			builder.Services.AddTransient<GenDbController<TwitterRecord, TwitterRecordDTO>>();
			builder.Services.AddTransient<AuthUserDbController>();
            builder.Services.AddTransient<EventLogicManager>(); // TwitterHelper
            builder.Services.AddTransient<TwitterHelper>();




            var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}


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
			app.MapHub<TwitterRecordHub<TwitterRecord, TwitterRecordDTO>>("/TwitterRecords");

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