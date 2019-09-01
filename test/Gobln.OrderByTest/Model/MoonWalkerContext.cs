using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Gobln.OrderByTest.Model
{
    public class MoonWalkerContext : DbContext
    {
        public MoonWalkerContext()
        {
            Database.EnsureCreated();
        }

        public MoonWalkerContext(DbContextOptions options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public override void Dispose()
        {
            Database.EnsureDeleted();

            base.Dispose();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MoonWalker>().HasData(TestData.MoonWalkers);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlite("Data Source=MoonWalker.db");

            optionsBuilder
                .UseLoggerFactory(GetLoggerFactory())
                .EnableSensitiveDataLogging();
        }

        private ILoggerFactory GetLoggerFactory()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddLogging(builder =>
                        builder.AddDebug()
                        .AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Information)
                        );

            return serviceCollection.BuildServiceProvider().GetService<ILoggerFactory>();
        }

        public DbSet<MoonWalker> MoonWalkers { get; set; }
    }
}
