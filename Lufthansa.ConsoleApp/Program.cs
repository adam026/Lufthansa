using Lufthansa.Data;
using Lufthansa.Logic;
using Lufthansa.Repository;

namespace Lufthansa.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // something
            var airplane = default(Airplane);
            Console.WriteLine("Print sample empty object: ");

            string message = airplane != null ? airplane.ToString() : "<null>";
            Console.WriteLine(airplane?.ToString() ?? "null");

            // init DB 
            Console.WriteLine("init DB..");
            var logic = GetLogic();

            // All AggregatedFlownDistance summarized for a brand
            var brand = "Airbus A320";
            var c1 = logic.GetTotalAggregatedFlownDistanceFor(brand);

            Console.WriteLine($"summarized AggregatedFlownDistance for brand {brand}: {c1}");

            // Write to console list of Airplanes for a brand
            var brand2 = "Airbus A320";

            var c2 = logic.GetAirplanesOf(brand2);

            foreach (var airplane1 in c2)
            {
                Console.WriteLine(airplane1.ToString() ?? "null");
            }

            var c3 = logic.GetTotalAggregatedFlownDistanceForAllBrands();

            foreach (var item in c3)
            {
                Console.WriteLine(item.ToString() ?? "null");
            }

            Console.ReadLine();
        }

        public static AirplaneLogic GetLogic()
        {
            var dbContext = new AirplaneDbContext();
            var repo = new AirplaneRepository(dbContext);
            var brandRepo = new BrandRepository(dbContext);
            var logic = new AirplaneLogic(repo, brandRepo);
            return logic;
        }
    }
}