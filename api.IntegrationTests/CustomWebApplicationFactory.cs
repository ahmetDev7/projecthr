using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.Sqlite;
using Microsoft.AspNetCore.Builder;

namespace api.IntegrationTests
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {   
            base.ConfigureWebHost(builder);

            builder.ConfigureTestServices(services => {
                var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

                services.Remove(dbContextDescriptor);

                var dbConnectionDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbConnection));
                
                services.Remove(dbConnectionDescriptor);

                services.AddSingleton<DbConnection>(container => {
                    var connection = new SqliteConnection("DataSource=:memory:");
                    connection.Open();

                    return connection;
                });

                services.AddDbContext<AppDbContext>((container, options) =>
                {
                        var connection = container.GetRequiredService<DbConnection>();
                        options.UseSqlite(connection);
                });

                services.AddScoped<IStartupFilter, SeedDataStartupFilter>();
            });
            
            builder.UseEnvironment("Test");
        }

        public class SeedDataStartupFilter : IStartupFilter
        {
            private readonly IServiceProvider _serviceProvider;

            public SeedDataStartupFilter(IServiceProvider serviceProvider)
            {
                _serviceProvider = serviceProvider;
            }

            public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    db.Database.EnsureCreated();
                    Seeding.IntializeTestDB(db);
                }

                return next;
            }
        }
    }
}
