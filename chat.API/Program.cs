using chat.API;
using chat.API.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
var aa = "Logging:LogLevel:Default";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
// setup appsettings and inject as service.. IOptions<T> == singleton & immutable; IOptionsSnapshot<T> == scoped; IOptionsMonitor == singleton & mutable
var variables = builder.Configuration.GetSection("Variables");
builder.Services.Configure<Appsettings>(variables);
// cors
builder.Services.ConfigureCors(variables.Get<Appsettings>() ?? throw new InvalidOperationException());

builder.Services.ConfigureMySqlContext(builder.Configuration.GetConnectionString("sqlConnection") ?? throw new InvalidOperationException());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// useful for forwarding headers when behind a proxy like nginx
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});
app.UseStaticFiles();
app.UseCors(variables.Get<Appsettings>()?.CorsPolicyName ?? string.Empty);
app.UseAuthorization();
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

// model keys
// get Group
// delete group
// update group
// mock db
// test project
// add users to group
// get group users
// delete user from group
