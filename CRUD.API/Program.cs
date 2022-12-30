using System.Text;
using System.Text.Json;
using CRUD.API;
using CRUD.BusinessLayer.Interfaces;
using CRUD.Dtos;
using CRUD.Dtos.Request;
using CRUD.Dtos.Response.User;
using CRUD.Utility.CommonHelper;
using CRUD.Utility.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;

var logger = new LoggerConfiguration()
	.MinimumLevel.Debug()
	.WriteTo.File("logs/AttorneyLogs/Attorney-.txt", rollingInterval: RollingInterval.Day,
		outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
	.CreateLogger();

var builder = WebApplication.CreateBuilder(args);

var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
new ConfigurationBuilder()
	.AddJsonFile($"appsettings.{environmentName}.json", true, true)
	.AddEnvironmentVariables();

builder.Host.UseSerilog(logger);

// Add services to the container.
builder.Services.AddCors();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// JWT token configuration
var configAppSettings = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(configAppSettings);

var appSettings = configAppSettings.Get<AppSettings>();
var key = Encoding.ASCII.GetBytes(appSettings.ApiKey!);

builder.Services.AddSwaggerGenNewtonsoftSupport();

builder.Services.AddAuthentication(option =>
{
	option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
	x.RequireHttpsMetadata = false;
	x.SaveToken = false;
	x.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuerSigningKey = true,
		IssuerSigningKey = new SymmetricSecurityKey(key),
		ValidateIssuer = false,
		ValidateAudience = false,
		ValidateLifetime = true
	};
});

// Required authentication for all calls.
builder.Services.AddAuthorization(config =>
{
	config.FallbackPolicy = new AuthorizationPolicyBuilder()
		.RequireAuthenticatedUser()
		.Build();
});

// Swagger configuration
builder.Services.AddSwaggerGen(c =>
{
	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
		Name = "Authorization",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer"
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
				},
				Scheme = "oauth2",
				Name = "Bearer",
				In = ParameterLocation.Header
			},
			new List<string>()
		}
	});
});

// Configure services
builder.Services.DependencySetting();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI(option =>
{
	option.DocExpansion(DocExpansion.None);
});

app.UseCors(x => x.AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowed(_ => true).AllowCredentials());
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// API code start

app.MapPost("User/GetUsers", async (ILogger<IUserService> log, IUserService userService) =>
{
	Response<List<UserResponse>> response = new();

	try
	{
		log.LogInformation("Method: GetUsers start");
		
		var detail = await userService.GetAllUser();

		response.Result = detail;
		response.Status = ResponseStatus.Success;

		log.LogInformation("Method: GetUsers end");
	}
	catch (Exception ex)
	{
		log.LogError(ex.Message);
		response.Status = ResponseStatus.Fail;
		response.Exception = new ExceptionResponse(ex, false, "User/");
		log.LogError(ex, "User/");
	}

	return response;

}).WithTags("User").AllowAnonymous();

app.MapPost("User/InsertUpdateDeleteUser", async (HttpRequest request, UserRequest userRequest ,ILogger<IUserService> log, IUserService userService) =>
{
	Response<List<UserResponse>> response = new();

	try
	{
		var jsonString = JsonSerializer.Serialize(userRequest);

		log.LogInformation("Method: InsertUpdateDeleteUser start");
		log.LogInformation($"Method: InsertUpdateDeleteUser request parameter: {jsonString}");

		userRequest.JwtToken = request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
		await userService.InsertUpdateDeleteUser(userRequest);

		response.Status = ResponseStatus.Success;

		log.LogInformation("Method: InsertUpdateDeleteUser end");
	}
	catch (Exception ex)
	{
		log.LogError(ex.Message);
		response.Status = ResponseStatus.Fail;
		response.Exception = new ExceptionResponse(ex, false, "User/");
		log.LogError(ex, "User/");
	}

	return response;

}).WithTags("User").AllowAnonymous();


app.Run();