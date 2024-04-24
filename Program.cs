using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SignalRSample.Data;
using SignalRSample.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
	.AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

//setup azure signal R
var azureConnection = "Endpoint=https://tk-test-sigr.service.signalr.net;AccessKey=4pNABEA4d1vfXtj3gGLfOR/A08EdTssDq8HRW+AmLdw=;Version=1.0;";
builder.Services.AddSignalR().AddAzureSignalR(azureConnection);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
}
else
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
	pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapHub<UserHub>("/hubs/userCount");
app.MapHub<RaceHub>("/hubs/RaceVotes");
app.MapHub<GroupNotificationHub>("/hubs/harryHouses");
app.MapHub<ChatNotificationHub>("/hubs/chat");
app.MapHub<ChatHub>("/hubs/chathub");
app.MapHub<OrderHub>("/hubs/order");

app.MapRazorPages();

app.Run();
