using Employee_Self_Service_BAL.Helper;
using Employee_Self_Service_BAL.Implementation;
using Employee_Self_Service_BAL.Interface;
using Employee_Self_Service_DAL.Data;
using Employee_Self_Service_DAL.Implementation;
using Employee_Self_Service_DAL.Interface;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var  connectionstring = builder.Configuration.GetConnectionString("DefaultConnnection");
builder.Services.AddDbContext<EmployeeSelfServiceContext>(q=>q.UseNpgsql(connectionstring));

// Add services to the container.
builder.Services.AddControllersWithViews();

// builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IEmailService, EmailService>();
// builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<ILeaveRepository, LeaveRepository>();
builder.Services.AddScoped<ILeaveService, LeaveService>();

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
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
