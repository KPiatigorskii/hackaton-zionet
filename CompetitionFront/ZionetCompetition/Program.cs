using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using ZionetCompetition.Data;
using BlazorStrap;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BlazorBootstrap;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();//.AddRazorRuntimeCompilation();
builder.Services.AddDbContext<ZionetCompetitionContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ZionetCompetitionContext") ?? throw new InvalidOperationException("Connection string 'ZionetCompetitionContext' not found.")));
builder.Services.AddServerSideBlazor();
builder.Services.AddBlazorBootstrap();
//builder.Services.AddBootstrapCSS();
//builder.Services.AddSingleton<WeatherForecastService>();

builder.Services
    .AddAuth0WebAppAuthentication(options => {
        options.Domain = builder.Configuration["Auth0:Domain"];
        options.ClientId = builder.Configuration["Auth0:ClientId"];
    });

builder.Services.AddHttpClient();



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

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
