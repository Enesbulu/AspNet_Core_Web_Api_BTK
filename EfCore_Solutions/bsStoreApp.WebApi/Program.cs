using bsStoreApp.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Services.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config")); //Nlog config dosyas�n�n tan�mlanmas�.
builder.Services.AddControllers(config =>
{
    config.RespectBrowserAcceptHeader = true;   //i�erik pazarlamaya a��k hale getirilmesi i�lemi.
    config.ReturnHttpNotAcceptable = true; // bu �ekilde d�n�� kodunda uygun hata kodunun verilmesi sa�land�. PAzarl�k yapmaya ba�lar. --HTTP406 
})
    .AddCustomCsvFormatter()    //Csv format�nda ��kt� verebilecek �ekilde yap�lan altyap�n�n extentions ile IOC ye eklenmesi.
    .AddXmlDataContractSerializerFormatters()   //XML Format�nda serilazer i�lemi yapmak i�in IOC ye kay�t yap�ld�.
    .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly)
    .AddNewtonsoftJson();   //Patch i�lem tipi i�in gerekli paket configuration yap�ld�.

//builder.Services.AddScoped<ValidationFilterAttribute>();    //Custsom Validation Attribute yap�s�n�n IOC ye kayd�n�n yap�lmas�
builder.Services.ConfigureActionFilters(); //Custom olarak yaz�lan Filter Attribute yap�lar�n�n toplu olarak IOC ye kayd�n�n yap�llmas�.
builder.Services.ConfigureCors();   //X-Pagination kullan�m�/Paginati�n i�in gerekli eklentinin dahil edilmesi.
builder.Services.Configure<ApiBehaviorOptions>(opt =>
{
    opt.SuppressModelStateInvalidFilter = true;
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager(); //Manager Yap�s� IOC ye kayd� yap�l�r.
builder.Services.ConfigureLoggerService();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.ConfigureDataShaper();

var app = builder.Build();
var logger = app.Services.GetRequiredService<ILoggerService>();
app.ConfigureExceptionHandler(logger);


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
if (app.Environment.IsProduction())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthorization();

app.MapControllers();

app.Run();
