
using Microsoft.EntityFrameworkCore;
using MsSqlAccessor.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.Extensions.Hosting;
using MsSqlAccessor.Hubs;

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
				//endpoints.MapHub<TestHub>("/users");
			});
            app.MapHub<TestHub>("/users");
            //app.MapHub<MssqlHub<User>>("/users");

            app.MapControllers();

			app.Run();
		}
	}
}