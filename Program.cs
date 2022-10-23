using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Web.Mvc;
using task.Models;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<FolderContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    //Creating Digital Images/
    //{controller}/{action}/
    pattern: "{**folders}",
    defaults: new { controller = "FolderModels", action = "Index", id = UrlParameter.Optional });

app.Run();
