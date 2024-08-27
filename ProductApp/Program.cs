var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.ClearProviders();   //Loglama için tanýmlanmýþ bir servis yapýsý. Default olarak loglama için tanýmlama yapýlmýþ oluyor. Ekstra olarak IOC tanýmlama yapmak gerekmiyor.
builder.Logging.AddConsole();   //Console'a log düþmek için.
builder.Logging.AddDebug(); //Debug ortamýna log düþme iþlemi için ekleme konfigurasyonlarý.

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
