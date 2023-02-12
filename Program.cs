using AspWebApiGlebTest.Models;
using AspWebApiGlebTest.Repository.cs;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AspWebApiGlebTest
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);


			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen((c) => 
			{
				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				c.IncludeXmlComments(xmlPath);
			});

			var connectionString = builder.Configuration.GetConnectionString("Default");

			builder.Services.AddScoped<IContactRepository, ContactRepositoryEFCore>();
			builder.Services.AddDbContext<ContactsDbContext>(options => 
			{
				options.UseSqlServer(connectionString);
			});

			var app = builder.Build();
			app.UseSwagger();
			app.UseSwaggerUI();

			app.MapControllers();

			app.Run();
		}
	}
}