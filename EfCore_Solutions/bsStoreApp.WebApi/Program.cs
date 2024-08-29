using bsStoreApp.WebApi.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson();   //Patch i�lem tipi i�in gerekli paket configuration yap�ld�.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<RepositoryContext>(   ///Olu�turulmu� repository bilgisi burada tan�mlan�r ve generic olarak repository class verilir.
    db => db.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection")  ///appsettings.json i�erisinde verdi�imiz connectionstring tan�mlamas�n� burada tan�mlayarak kullan�ma al�yoruz. ---IOC(Inversion of Control - Konrol� Tersine �evirme)  dahilinde yap�lm��t�r.
));

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
