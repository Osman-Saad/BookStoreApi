using BookStore.Core.Models;
using BookStore.Repository.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddIdentity<AppUser,IdentityRole>()
    .AddEntityFrameworkStores<BookStoreDbContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();

var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var loggerFactory = services.GetRequiredService<ILoggerFactory>();

try
{
    var dbContext = services.GetRequiredService<BookStoreDbContext>();
    await dbContext.Database.MigrateAsync();
}
catch(Exception ex) 
{
    var logger = loggerFactory.CreateLogger<Program>();
    logger.LogError(ex.Message);
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
