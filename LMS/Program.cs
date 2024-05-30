global using LMS.Models;
global using LMS.Data;
using Microsoft.EntityFrameworkCore;
using LMS.Repository;
using LMS.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Extensions.Configuration;
using LMS.Hubs;
using static LMS.Hubs.MyHub;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IResourceService, ResourceService>();
builder.Services.AddScoped<INotificationService,NotificationService>();
builder.Services.AddScoped<IRequestService, RequestService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IDashboardService,DashboardService>();
builder.Services.AddScoped<JWTService>();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is a very secure key for me")),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero
    };
});



// Add Hangfire services.
builder.Services.AddHangfire((sp, config) =>
{
    var connectionstring = sp.GetRequiredService<IConfiguration>().GetConnectionString("Server=.;Database=HangFire;Integrated Security=True;TrustServerCertificate=true");
    config.UseSqlServerStorage("Server=.;Database=HangFire;Integrated Security=True;TrustServerCertificate=true");

});
builder.Services.AddHangfireServer();

builder.Services.AddSignalR();

// Add the processing server as IHostedService

builder.Services.AddAuthorization();
builder.Services.AddCors();

builder.Services.AddDbContext<DataContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DataContext")));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(policy => policy.AllowAnyHeader()
                            .AllowAnyMethod()
                            .SetIsOriginAllowed(origin => true)
                            .AllowCredentials());



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHangfireDashboard();
app.UseHttpsRedirection();

app.UseCors(options=>options
    .WithOrigins( "http://localhost:3000")
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    );


app.UseAuthentication();
app.UseAuthorization();
app.UseHangfireDashboard();
app.MapHub<MessagingHub>("/Hubs/MyHub");
app.MapControllers();

app.Run();
