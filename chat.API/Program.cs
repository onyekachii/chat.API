using chat.API;
using chat.API.Extensions;
using chat.Domain.DTOs;
using chat.Repo;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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

// ---- Authentication (JWT) ----
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Auth:Authority"];
        options.Audience = builder.Configuration["Auth:Audience"];

        // allow token in querystring for WebSockets/negotiate
        var originalOnMessage = options.Events.OnMessageReceived;
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = async context =>
            {
                // first run original
                if (originalOnMessage != null) await originalOnMessage(context);

                var accessToken = context.Request.Query["access_token"].FirstOrDefault();
                var path = context.HttpContext.Request.Path;

                if (!string.IsNullOrEmpty(accessToken) &&
                    path.StartsWithSegments("/hubs/chat"))
                {
                    context.Token = accessToken;
                }
            }
        };
    });

builder.Services.AddAuthorization();

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
app.UseAuthentication();
app.UseAuthorization();

app.MapHub<ChatHub>("/hubs/chat");

app.MapPost("api/app/create", async (IRepoFactory factory, AppDTO app) =>
{
    var result = await factory.App.CreateAsync(AppDTO.mapDtoToApp(app));
    await factory.SaveAsync();
    return Results.Ok(result.Entity);
}).WithName("CreateApp").WithTags("App").WithOpenApi();

app.MapPost("api/group/create", async (IRepoFactory factory, GroupDTO group) =>
{
    var result = await factory.Group.CreateAsync(GroupDTO.mapDtoToGroup(group));
    await factory.SaveAsync();
    return Results.Ok(result.Entity);
}).WithName("CreateGroup").WithTags("Group").WithOpenApi();

app.MapGet("api/group/getbyname", async (IRepoFactory factory, string name) =>
{
    var result =  factory.Group.FindByCondition(g => g.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
    await factory.SaveAsync();
    return Results.Ok(result);
}).WithName("GetGroupByName").WithTags("Group").WithOpenApi();

app.MapPost("api/user/create", async (IRepoFactory factory, UserDTO user) =>
{
    var result = await factory.User.CreateAsync(UserDTO.mapDtoToUser(user));
    await factory.SaveAsync();
    return Results.Ok(result.Entity);
}).WithName("CreateUser").WithTags("User").WithOpenApi();

//app.MapGet("/api/group/{group}/messages", async (string group, ChatDbContext db, [FromQuery] int take = 50) =>
//{
//    var msgs = await db.Messages
//        .Where(m => m.Room == room)
//        .OrderByDescending(m => m.SentAt)
//        .Take(take)
//        .OrderBy(m => m.SentAt) // return oldest->newest
//        .ToListAsync();
//    return Results.Ok(msgs);
//}).RequireAuthorization();
app.Run();

// migration
// delete group
// update group
// test project
// add users to group
// get group users
// delete user from group