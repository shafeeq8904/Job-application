using JobTrackerAPI.Data;
using Microsoft.EntityFrameworkCore;
using JobTrackerAPI.Repositories;
using JobTrackerAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


#region services
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IJobApplicationService, JobApplicationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IStatusLogService, StatusLogService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IJobPostingService, JobPostingService>();



#endregion


#region cors
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
              .WithOrigins(
                  "http://127.0.0.1:4200",
                  "http://localhost:4200"
              );
    });
});
#endregion

var app = builder.Build();
app.UseSwagger();
app.UseCors();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();
app.Run();
