using BooksRestAPI.Models;
using BooksRestAPI.Models.DataAccess.Contract;
using BooksRestAPI.Models.DataAccess.Implementation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Reflection;
// <summary>
// program file in .NET 6, include all needed for Startup.cs in the .NET 5
// This a a program for books, get, post, put, delete 
//</summary>

var builder = WebApplication.CreateBuilder(args);

string connString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BookDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

});
// Add services to the container.
builder.Services.AddOptions();

builder.Services.AddTransient<IBookRepo, BookRepo>();
builder.Services.AddScoped<IBookRepo, BookRepo>();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Books Rest API",
        Description = "A simple API to create a books",
        Contact = new OpenApiContact
        {
            Name = "Mehrdad Zandi",
            Email = "mehrdad.zandi@softsolutionsahand.com",
            Url = new Uri("http://www.softsolutionsahand.com/")
        }
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
// move these out of if statment to upload Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Book API V1");
    c.RoutePrefix = string.Empty;
});

app.UseAuthorization();

app.MapControllers();

app.Run();

