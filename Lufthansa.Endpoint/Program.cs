using Lufthansa.Data;
using Lufthansa.Logic;
using Lufthansa.Repository;

namespace Lufthansa.Endpoint
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var services = builder.Services;
            services.AddSingleton<IDbContext<Brand>, AirplaneDbContext>();
            services.AddSingleton<IDbContext<Airplane>, AirplaneDbContext>();

            services.AddSingleton<IRepository<Airplane>, Repository<Airplane>>();
            services.AddSingleton<IRepository<Brand>, Repository<Brand>>();

            services.AddSingleton<IAirplaneLogic, AirplaneLogic>();


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}