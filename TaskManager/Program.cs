using TaskManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("TaskManagerDbContextConnection") ?? throw new InvalidOperationException("Connection string 'TaskManagerDbContextConnection' not found.");
// Add services to the container.

builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<TaskManagerDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration["ConnectionStrings:TaskManagerDbContextConnection"]);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
/*if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}*/
if(app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();



app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
