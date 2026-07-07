using Microsoft.AspNetCore.Authentication;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MyFirstWebAPI;
using MyFirstWebAPI.Authentication;
using MyFirstWebAPI.Data;
using MyFirstWebAPI.Middlewares;
using MyFirstWebAPI.Middlewares.Filters;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("config.json");

builder.Services.Configure<AttachmentOptions>(builder.Configuration.GetSection("AttachmentOptions"));

// Add services to the container.

builder.Services.AddControllers(options =>
{
    // by putting the filter here, it will be applied to all controllers and actions in the application (global filter)
    options.Filters.Add<LogActivityFilter>();
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(cfg => cfg.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]));

builder.Services.AddAuthentication()
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", null);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); 
}

app.UseMiddleware<RateLimitingMiddleware>();
app.UseMiddleware<ProfilingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
