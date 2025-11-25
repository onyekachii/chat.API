using chat.API;
using chat.API.Extensions;
using chat.API.MiddleWare;
using chat.Domain.DTOs;
using chat.Repo;
using chat.Service;
using chat.Service.Implementation;
using chat.Service.Interface;
using chat.Service.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var variables = builder.Configuration.GetSection("Variables");
var jwt = builder.Configuration.GetSection("Jwt");
var connString = builder.Configuration.GetConnectionString("sqlConnection") ?? throw new InvalidOperationException();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{   
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT token into the field. Example: {token}",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
builder.Services.AddSignalR();
builder.Services.AddControllers();
// setup appsettings and inject as service.. IOptions<T> == singleton & immutable; IOptionsSnapshot<T> == scoped; IOptionsMonitor == singleton & mutable
builder.Services.Configure<Appsettings>(variables);
builder.Services.Configure<JwtConfig>(jwt);
// cors
builder.Services.ConfigureCors(variables.Get<Appsettings>() ?? throw new InvalidOperationException());
builder.Services.AddDbContext<ChatContext>(o => o.UseMySql(connString, MySqlServerVersion.LatestSupportedServerVersion));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAppService, AppService>();
builder.Services.AddScoped<IGroupService, GroupService>();
//  Authentication (JWT) 
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.SaveToken = true; // save in memory per request
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt.Get<JwtConfig>()?.Issuer,
            ValidAudience = jwt.Get<JwtConfig>()?.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Get<JwtConfig>().Key)),
            ClockSkew = TimeSpan.Zero
        };


        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                System.Diagnostics.Debug.WriteLine("AUTH FAILED: " + context.Exception.Message);
                return Task.CompletedTask;
            },

            OnMessageReceived = context =>
            {
                System.Diagnostics.Debug.WriteLine("TOKEN RECEIVED: " + context.Token);
                return Task.CompletedTask;
            },

            OnTokenValidated = context =>
            {
                System.Diagnostics.Debug.WriteLine("TOKEN VALIDATED OK");
                return Task.CompletedTask;
            }
        };
        //// allow token in querystring for WebSockets/negotiate
        //var originalOnMessage = options.Events.OnMessageReceived;
        //options.Events = new JwtBearerEvents
        //{
        //    OnMessageReceived = async context =>
        //    {
        //        // first run original
        //        if (originalOnMessage != null) await originalOnMessage(context);

        //        var accessToken = context.Request.Query["access_token"].FirstOrDefault();
        //        var path = context.HttpContext.Request.Path;

        //        if (!string.IsNullOrEmpty(accessToken) &&
        //            path.StartsWithSegments("api/hubs/chat"))
        //        {
        //            context.Token = accessToken;
        //        }
        //    }
        //};
    })
    .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthHandler>(
        ApiKeyAuthHandler.SchemeName, o => { });
    

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
app.MapControllers();



#region ENDPOINTS
app.MapHub<ChatHub>("api/hubs/chat");

app.MapPost("api/app/create", async (IAppService service, IUnitOfWork uow, [FromBody] AppDTO app) =>
{
    var result = await service.CreateAppAsync(app, "");
    await uow.SaveAsync();
    return Results.Ok(result);
}).WithName("CreateApp").WithTags("App").WithOpenApi().RequireAuthorization();

app.MapGet("api/group/getbyname", async (IUnitOfWork factory, string name) =>
{
    var result =  factory.Group.FindByCondition(g => g.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
    await factory.SaveAsync();
    return Results.Ok(result);
}).WithName("GetGroupByName").WithTags("Group").WithOpenApi();


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

// delete group
// update group
// test project
// add users to group
// get group users
// delete user from group

#endregion


app.Run();