global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using WorkerServiceTutorial.WebApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BlogDBContext>(options => options.UseSqlServer(GetConnectionString(builder.Configuration)));
static string GetConnectionString(ConfigurationManager? configurationManager)
{
    if (configurationManager == null) return "";
    var cnString = $"{configurationManager.GetConnectionString("DefaultConnection")}";
    var retVal = cnString.Replace("$PASSWORD$", Environment.GetEnvironmentVariable("SA_PASSWORD"));
    retVal = retVal.Replace("$DB_SERVER$", Environment.GetEnvironmentVariable("DB_SERVER"));
    return retVal;
}

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthorization();
app.MapControllers();
app.Run();

