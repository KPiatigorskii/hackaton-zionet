
using Microsoft.EntityFrameworkCore;
using MsSqlAccessor.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.Extensions.Hosting;
using MsSqlAccessor.Hubs;
using MsSqlAccessor.DbControllers;
using Task = MsSqlAccessor.Models.Task;
using MsSqlAccessor.Services;
using Owin;

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
            builder.Services.AddSignalR(hubOptions =>
            {
                hubOptions.EnableDetailedErrors = true;
                hubOptions.KeepAliveInterval = System.TimeSpan.FromMinutes(1);
            });

            builder.Services.AddDbContext<CompetitionBdTestContext>();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
            //builder.Services.AddTransient<IUserRole, MockUserRole>();
            builder.Services.AddTransient<EventsDbController>();

            var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();


			app.UseRouting();
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
            app.MapHub<EventHub>("/events");
			app.MapHub<MsSQLHub<EventManager, EventManagerDTO>>("/EventManagers");
			app.MapHub<MsSQLHub<EventParticipantTeam, EventParticipantTeamDTO>>("/eventparticipantteams");
			app.MapHub<MsSQLHub<EventTask, EventTaskDTO>>("/EventTasks");
			app.MapHub<MsSQLHub<EventTaskEvaluateUser, EventTaskEvaluateUserDTO>>("/EventTaskEvaluateUsers");
			app.MapHub<MsSQLHub<Role, RoleDTO>>("/Roles");
			app.MapHub<MsSQLHub<Status, StatusDTO>>("/Statuses");
			app.MapHub<MsSQLHub<Task, TaskDTO>>("/Tasks");
			app.MapHub<MsSQLHub<TaskCategory, TaskCategoryDTO>>("/TaskCategories");
			app.MapHub<MsSQLHub<TaskParticipant, TaskParticipantDTO>>("/TaskParticipants");
			app.MapHub<MsSQLHub<Team, TeamDTO>>("/Teams");
			app.MapHub<MsSQLHub<TeamTask, TeamTaskDTO>>("/TeamTasks");
			app.MapHub<MsSQLHub<User, UserDTO>>("/Users");

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