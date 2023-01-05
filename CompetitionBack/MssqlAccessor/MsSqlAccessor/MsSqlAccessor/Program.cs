
using Microsoft.EntityFrameworkCore;
using MsSqlAccessor.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.Extensions.Hosting;
using MsSqlAccessor.Hubs;
using MsSqlAccessor.DbControllers;

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
            builder.Services.AddSignalR();

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
            app.MapHub<EventHub>("/events");
            //app.MapHub<EventParticipantTeamHub>("/eventparticipantteams");
            app.MapHub<MsSQLHub<EventParticipantTeam, EventParticipantTeamDTO>>("/eventparticipantteams");
            app.MapHub<MsSQLHub<User, UserDTO>>("/users");
            //app.MapHub<MssqlHubOld<User>>("/users");

            app.MapControllers();

			app.Run();
		}
	}
}