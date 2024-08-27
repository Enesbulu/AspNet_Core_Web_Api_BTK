var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.ClearProviders();   //Loglama i�in tan�mlanm�� bir servis yap�s�. Default olarak loglama i�in tan�mlama yap�lm�� oluyor. Ekstra olarak IOC tan�mlama yapmak gerekmiyor.
builder.Logging.AddConsole();   //Console'a log d��mek i�in.
builder.Logging.AddDebug(); //Debug ortam�na log d��me i�lemi i�in ekleme konfigurasyonlar�.

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
