using Microsoft.EntityFrameworkCore;
using PagLogo;
using PagLogo.Models;
using PagLogo.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options => 
        options.UseSqlServer(@"Server=localhost,1433; Database=TestDB; 
                User ID = SA; Password = Root@123; Trusted_Connection = False")
);
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAppDbContext, AppDbContext>();

var app = builder.Build();

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
