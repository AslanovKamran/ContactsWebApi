using AspWebApiGlebTest.Data;
using AspWebApiGlebTest.Repository.Dapper;
using AspWebApiGlebTest.Repository.EFCore;
using AspWebApiGlebTest.Repository.Interfaces;
using AspWebApiGlebTest.Tokens;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using System.Reflection;
using System.Text;

namespace AspWebApiGlebTest
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen((options) =>
			{
				#region Documentation Section

				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				options.IncludeXmlComments(xmlPath);

				#endregion

				#region Jwt Bearer Section
				var securityScheme = new OpenApiSecurityScheme
				{
					Name = "Jwt Authentication",
					Description = "Type in a valid JWT Bearer",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.Http,
					Scheme = "Bearer",
					BearerFormat = "Jwt",
					Reference = new OpenApiReference
					{
						Id = JwtBearerDefaults.AuthenticationScheme,
						Type = ReferenceType.SecurityScheme
					}
				};
				options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
				options.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{securityScheme,Array.Empty<string>() }
				});

				#endregion
			});

			builder.Services.AddHttpLogging(httpLogging =>
			{
				httpLogging.LoggingFields = HttpLoggingFields.All;
			});


			#region Jwt Options
			var jwtOptions = builder.Configuration.GetSection("Jwt");
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions["Key"]!));

			const double AccessTokenLifeTime = 3;
			const double RefreshokenLifeTime = 30;

			builder.Services.Configure<JwtOptions>(options =>
			{
				options.Issuer = jwtOptions["Issuer"]!;
				options.Audience = jwtOptions["Audience"]!;
				options.AccessValidFor = TimeSpan.FromMinutes(AccessTokenLifeTime);
				options.RefreshValidFor = TimeSpan.FromDays(RefreshokenLifeTime);
				options.SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
			});
			#endregion

			#region Authentication
			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateIssuerSigningKey = true,
					ValidateLifetime = true,

					ValidIssuer = builder.Configuration["Jwt:Issuer"],
					ValidAudience = builder.Configuration["Jwt:Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
					
					ClockSkew = TimeSpan.Zero // Removes default additional time to the tokens
				};
			});
			#endregion

			var connectionString = builder.Configuration.GetConnectionString("Default");
			
			#region Dapper 
			//builder.Services.AddScoped<IContactRepository, ContactRepositoryDapper>(provider => new ContactRepositoryDapper(connectionString!));
			//builder.Services.AddScoped<IUserRepository, UserRepositoryDapper>(provider => new UserRepositoryDapper(connectionString!));
			#endregion

			#region EF Core
			builder.Services.AddDbContext<ContactsTestDbContext>(options => 
			{
				options.UseSqlServer(connectionString);
			});

			builder.Services.AddScoped<IContactRepository, ContactRepositoryEfCore>();
			builder.Services.AddScoped<IUserRepository, UserRepositoryEfCore>();

			#endregion

			builder.Services.AddSingleton<ITokenGenerator, TokenGenerator>();

			var app = builder.Build();

			app.UseSwagger();
			app.UseSwaggerUI();

			app.UseHttpLogging();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}