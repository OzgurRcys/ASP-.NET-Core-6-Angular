using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Project2API.API.Extensions;
using Project2API.Application;
using Project2API.Application.Validators.Products;
using Project2API.Infrastructure;
using Project2API.Infrastructure.Filters;
using Project2API.Infrastructure.Services.Storage.Azure;
using Project2API.Infrastructure.Services.Storage.Local;
using Project2API.Persistence;
using Project2API.SignalR;
using Project2API.SignalR.Hubs;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Data;
using System.Data.SqlClient;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices();

builder.Services.AddStorage<AzureStorage>();
//builder.Services.AddStorage<LocalStorage>();
//builder.Services.AddStorage();
builder.Services.AddApplicationServices();
builder.Services.AddSignalRServices();

builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.WithOrigins("http://localhost:4200", "https://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowCredentials()));

Logger log = new LoggerConfiguration().WriteTo.Console().WriteTo.File("logs/log.txt").WriteTo.MSSqlServer(builder.Configuration.GetConnectionString("Project2ConnectionString"), "logs", autoCreateSqlTable: true).WriteTo.Seq(builder.Configuration["Seq:ServerURL"]).Enrich.FromLogContext().MinimumLevel.Information().CreateLogger();
           


//Logger log = new LoggerConfiguration().WriteTo.Console().WriteTo.File("logs/log.txt").WriteTo.MSSqlServer(builder.Configuration.GetConnectionString("Project2ConnectionString"), sinkOptions: new MSSqlServerSinkOptions { TableName = "Log", AutoCreateSqlTable = true }
//, null, null, LogEventLevel.Warning, null, null, null, null).CreateLogger();

//Logger log = new LoggerConfiguration().WriteTo.Console().WriteTo.File("logs/log.txt").WriteTo.PostgreSQL(builder.Configuration.GetConnectionString("Project2ConnectionString"), "logs", needAutoCreateTable: true, columnOptions: new Dictionary<string, ColumnWriterBase>
//{
//}).CreateLogger();


builder.Host.UseSerilog(log);

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("sec-ch-ua");
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;

});

builder.Services.AddControllers(options => options.Filters.Add<ValidationFilter>())
    .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>()).ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer("Admin", options =>
{
    options.TokenValidationParameters = new()
    {
        ValidateAudience = true, //oluþturulan token deðeri hangi sitelerde kullanýlacak
        ValidateIssuer = true, // oluþturulan token deðerini hangi sitenin daðýttýðýný ifade eder.
        ValidateLifetime = true, //token geçerlilik süresi.
        ValidateIssuerSigningKey = true, // tokenin uygulamaya ait mi olduðunu ifade eder.

        ValidAudience = builder.Configuration["Token:Audience"],
        ValidIssuer = builder.Configuration["Token:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecuriyKey"])),
        LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow : false, //tokeni belirtilen süre içersinde yaþatma iþlemi
        NameClaimType = ClaimTypes.Name //jwt üzerinde nama claimine karþýlýk gelen deðeri user.identity.name propertysinden elde edebiliriz.
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureExceptionHandler<Program>(app.Services.GetRequiredService<ILogger<Program>>());
app.UseStaticFiles();

app.UseSerilogRequestLogging();

app.UseHttpLogging();
app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.Use(async(context, next) =>
{
    var username = context.User.Identity?.IsAuthenticated != null || true ? context.User.Identity.Name : null;
    LogContext.PushProperty("user_name",username);
    await next();
});

app.MapControllers();
app.MapHubs();

app.Run();
