using bsStoreApp.WebApi.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson();   //Patch iþlem tipi için gerekli paket configuration yapýldý.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<RepositoryContext>(   ///Oluþturulmuþ repository bilgisi burada tanýmlanýr ve generic olarak repository class verilir.
    db => db.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection")  ///appsettings.json içerisinde verdiðimiz connectionstring tanýmlamasýný burada tanýmlayarak kullanýma alýyoruz. ---IOC(Inversion of Control - Konrolü Tersine Çevirme)  dahilinde yapýlmýþtýr.
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
