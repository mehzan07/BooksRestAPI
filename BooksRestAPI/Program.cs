using BooksRestAPI.Models;
using BooksRestAPI.Models.DataAccess.Contract;
using BooksRestAPI.Models.DataAccess.Implementation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

string connString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BookDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

});
// Add services to the container.
builder.Services.AddTransient<IBookRepo, BookRepo>();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

