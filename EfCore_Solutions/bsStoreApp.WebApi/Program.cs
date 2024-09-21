using bsStoreApp.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Services.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config")); //Nlog config dosyasýnýn tanýmlanmasý.
builder.Services.AddControllers(config =>
{
    config.RespectBrowserAcceptHeader = true;   //içerik pazarlamaya açýk hale getirilmesi iþlemi.
    config.ReturnHttpNotAcceptable = true; // bu þekilde dönüþ kodunda uygun hata kodunun verilmesi saðlandý. PAzarlýk yapmaya baþlar. --HTTP406 
})
    .AddCustomCsvFormatter()    //Csv formatýnda çýktý verebilecek þekilde yapýlan altyapýnýn extentions ile IOC ye eklenmesi.
    .AddXmlDataContractSerializerFormatters()   //XML Formatýnda serilazer iþlemi yapmak için IOC ye kayýt yapýldý.
    .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly)
    .AddNewtonsoftJson();   //Patch iþlem tipi için gerekli paket configuration yapýldý.

//builder.Services.AddScoped<ValidationFilterAttribute>();    //Custsom Validation Attribute yapýsýnýn IOC ye kaydýnýn yapýlmasý
builder.Services.ConfigureActionFilters(); //Custom olarak yazýlan Filter Attribute yapýlarýnýn toplu olarak IOC ye kaydýnýn yapýllmasý.
builder.Services.ConfigureCors();   //X-Pagination kullanýmý/Paginatiýn için gerekli eklentinin dahil edilmesi.
builder.Services.Configure<ApiBehaviorOptions>(opt =>
{
    opt.SuppressModelStateInvalidFilter = true;
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager(); //Manager Yapýsý IOC ye kaydý yapýlýr.
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
