using AdeAuth.Services;
using AdeAuth.Services.AuthServices;
using AdeAuth.Services.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var newUsers = DeployConfiguration.AddNewUsers();
var azureAd = builder.Configuration.GetSection("AzureAd");
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options => {
        options.SaveToken = true;
        options.MetadataAddress = "https://login.microsoftonline.com/organizations/v2.0/.well-known/openid-configuration";
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            NameClaimType = "name",
            ValidAudience = "c7353b92-fa70-41f9-9ca5-8dc6ceb15dca",
            ValidIssuer = "https://login.microsoftonline.com/fa23820c-5ae0-43de-968f-69e872bc6200/v2.0",
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true
        };});
builder.Services.AddAuthorization(config =>
{
    config.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
   .RequireAuthenticatedUser().Build();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
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
                        Array.Empty<string>()
                    }
                });
    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordManager, PasswordManager>();
builder.Services.AddScoped<IUserRepository>((_) => new UserRepository(newUsers));

builder.Services.AddCors(option =>
{
    option.AddPolicy("Policy", c => c.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment())
{
    
}*/

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("Policy");

app.UseAuthentication();

app.MapControllers();

app.Run();
