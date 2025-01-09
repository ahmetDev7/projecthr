using Microsoft.EntityFrameworkCore;

namespace api.IntegrationTests{
    public class DatabaseFixture : IDisposable
    {
        public AppDbContext Context { get; private set; }

        public DatabaseFixture()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            Context = new AppDbContext(options);

            Context.Database.EnsureCreated();
            Seeding.IntializeTestDB(Context);
        }

        public void Dispose()
        {
            Context.Database.EnsureDeleted();
            Context.Dispose();
        }
    }
}