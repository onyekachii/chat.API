using chat.API;
using chat.API.Extensions;
using chat.Domain.DTOs;
using chat.Domain.Entities;
using chat.Repo;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;

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

var connString = builder.Configuration.GetConnectionString("sqlConnection") ?? throw new InvalidOperationException();
builder.Services.AddDbContext<ChatContext>(o => o.UseMySql(connString, MySqlServerVersion.LatestSupportedServerVersion));

builder.Services.AddScoped<IRepoFactory, RepoFactory>();

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

app.MapPost("/app/create", async (IRepoFactory factory, AppDTO app) =>
{
    var result = await factory.App.CreateAsync(AppDTO.mapDtoToApp(app));
    return Results.Ok(result.Entity);
}).WithName("CreateApp").WithTags("App").WithOpenApi();

app.MapPost("/group/create", async (IRepoFactory factory, GroupDTO group) =>
{
    var result = await factory.Group.CreateAsync(GroupDTO.mapDtoToGroup(group));
    return Results.Ok(result.Entity);
}).WithName("CreateGroup").WithTags("Group").WithOpenApi();

app.MapGet("/group/getbyname", async (IRepoFactory factory, string name) =>
{
    var result =  factory.Group.FindByCondition(g => g.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
    return Results.Ok(result);
}).WithName("GetGroupByName").WithTags("Group").WithOpenApi();

app.MapPost("/user/create", async (IRepoFactory factory, UserDTO user) =>
{
    var result = await factory.User.CreateAsync(UserDTO.mapDtoToUser(user));
    return Results.Ok(result.Entity);
}).WithName("CreateUser").WithTags("User").WithOpenApi();

app.Run();

// delete group
// update group
// test project
// add users to group
// get group users
// delete user from group