using Services.Config;
using Services.Interfaces;
using Services.Services;

var builder = WebApplication.CreateBuilder(args);

AppSettings.AzureStorageConnectionString = builder.Configuration.GetSection("AppSettings")["AzureConnectionString:StorageConnectionString"];

builder.Services.AddScoped(typeof(INoSqlStorage<>), typeof(TableStorageService<>));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
