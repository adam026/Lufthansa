using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Lufthansa.Data
{
    public class AirplaneDbContext : DbContext, IDbContext<Airplane>, IDbContext<Brand>
    {
        public DbSet<Airplane> Airplanes { get; set; }
        public DbSet<Brand> Brands { get; set; }

        public AirplaneDbContext()
        {
            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string conn =
                    @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\LufthansaDB.mdf;Integrated Security=True";
                optionsBuilder
                    .UseLazyLoadingProxies()
                    .UseSqlServer(conn);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Airplane>()
                .HasOne(airplane => airplane.Brand)
                .WithMany(brand => brand.Airplanes)
                .HasForeignKey(airplane => airplane.BrandId);

            var brand1 = new Brand()
            {
                Id = 1,
                MaxFlightDistance = 1000,
                NumberOfPassengerSeat = 120,
                Name = "Boeing 747C",
            };

            var brand2 = new Brand()
            {
                Id = 2,
                MaxFlightDistance = 1500,
                NumberOfPassengerSeat = 80,
                Name = "Airbus A320",
            };

            var airplane = new Airplane()
            {
                Id = 1,
                AggregatedFlownDistance = 0,
                ProductionDate = new DateTime(2000, 1, 1),
                BrandId = 1,
            };
            
            var airplane2 = new Airplane()
            {
                Id = 2,
                AggregatedFlownDistance = 0,
                ProductionDate = new DateTime(2004, 1, 1),
                BrandId = 2,

            };

            var airplane3 = new Airplane()
            {
                Id = 3,
                AggregatedFlownDistance = 0,
                ProductionDate = new DateTime(2005, 1, 1),
                BrandId = 2,
            };

            modelBuilder.Entity<Brand>().HasData(brand1, brand2);
            modelBuilder.Entity<Airplane>().HasData(airplane, airplane2, airplane3);
        }

        DbSet<Airplane> IDbContext<Airplane>.GetDbSet()
        {
            return Airplanes;
        }

        IQueryable<Airplane> IDbContext<Airplane>.GetDbSetWithInclude()
        {
            return Airplanes.Include(airplane => airplane.Brand).AsQueryable();
        }
        IQueryable<Brand> IDbContext<Brand>.GetDbSetWithInclude()
        {
            return Brands.Include(brand => brand.Airplanes).AsQueryable();
        }

        DbSet<Brand> IDbContext<Brand>.GetDbSet()
        {
            return Brands;
        }
    }
}
