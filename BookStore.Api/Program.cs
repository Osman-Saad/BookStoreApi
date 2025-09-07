using BookStore.Api.Errors;
using BookStore.Api.Moddlewares;
using BookStore.Api.Profiles;
using BookStore.Core;
using BookStore.Core.IServices;
using BookStore.Core.Models;
using BookStore.Repository;
using BookStore.Repository.Data;
using BookStore.Repository.Data.Seeding;
using BookStore.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BookStoreDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddAutoMapper(typeof(MapperProfile));
builder.Services.AddIdentity<AppUser,IdentityRole>()
    .AddEntityFrameworkStores<BookStoreDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = actionContext =>
    {
        var errors = actionContext.ModelState
            .Where(e => e.Value.Errors.Count > 0)
            .SelectMany(x => x.Value.Errors)
            .Select(x => x.ErrorMessage).ToArray();
        var errorResponse = new ValidationErrorRespons()
        {
            Errors = errors
        };
        return new BadRequestObjectResult(errorResponse);
    };
});

var app = builder.Build();

var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var loggerFactory = services.GetRequiredService<ILoggerFactory>();

try
{
    var dbContext = services.GetRequiredService<BookStoreDbContext>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await dbContext.Database.MigrateAsync();
    await BookStoreSeedContext.SeedRolesAsync(roleManager);
}
catch(Exception ex) 
{
    var logger = loggerFactory.CreateLogger<Program>();
    logger.LogError(ex.Message);
}
app.UseMiddleware<ExceptionMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStatusCodePagesWithReExecute("/errors/{0}");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
