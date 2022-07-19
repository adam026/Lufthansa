using Lufthansa.Data;
using Lufthansa.Repository;

namespace Lufthansa.Logic.Tests
{
    [TestFixture]
    public partial class AirplaneLogicTests
    {
        
        //[OneTimeTearDown]
        //[OneTimeSetUp]
        
        [SetUp]
        public void Setup()
        {

        }

        [TearDown]
        public void TearDown()
        {

        }

        [Test]
        public void GetAirplanesOf_WithZeroRepoValues_ReturnsWithEmptyList()
        {
            //Arrange
            var airplaneRepo = new InMemoryRepository<Airplane>();
            var brandRepo =new InMemoryRepository<Brand>();
            var sut = new AirplaneLogic(airplaneRepo, brandRepo);

            //Act
            var result = sut.GetAirplanesOf("");

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void GetAirplanesOf_SuppliedBrandNameIsNull_ReturnsWithEmptyList()
        {
            //Arrange
            var airplaneRepo = new InMemoryRepository<Airplane>();
            var brandRepo = new InMemoryRepository<Brand>();
            var sut = new AirplaneLogic(airplaneRepo, brandRepo);

            //Act
            var result = sut.GetAirplanesOf(null);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(0));
        }

        [Test]
        public void Ctor_NullRepository_ThrowsArgumentNullException()
        {
            //Arrange
            var sut = Assert.Throws<ArgumentNullException>( () => new AirplaneLogic(null, null));

        }

        [Test]
        public void GetAirplanesOf_WithEmptyBrand_ReturnsWithEmptyList()
        {
            //Arrange
            var expectedBrandName = "KualaLumpur";
            var airplaneRepo = new InMemoryRepository<Airplane>();
            var brandRepo = new InMemoryRepository<Brand>();
            var brand = new Brand()
            {
                Id = 1,
                MaxFlightDistance = 3000,
                Name = expectedBrandName,
                NumberOfPassengerSeat = 200,
                Airplanes = new List<Airplane>()
            };

            var airplane = new Airplane()
            {
                Brand = brand,
                BrandId = brand.Id,
                Id = 1, 
                AggregatedFlownDistance = 0,
                ProductionDate = new DateTime(1995,1,1)
            };

            brand.Airplanes.Add(airplane);

            airplaneRepo.Create(airplane);
            brandRepo.Create(brand);

            var sut = new AirplaneLogic(airplaneRepo, brandRepo);

            //Act
            var result = sut.GetAirplanesOf("");

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void GetAirplanesOf_WithUnMatchingBrand_ReturnsWithEmptyList()
        {
            //Arrange
            var expectedBrandName = "KualaLumpur";
            var airplaneRepo = new InMemoryRepository<Airplane>();
            var brandRepo = new InMemoryRepository<Brand>();
            var brand = new Brand()
            {
                Id = 1,
                MaxFlightDistance = 3000,
                Name = expectedBrandName,
                NumberOfPassengerSeat = 200,
                Airplanes = new List<Airplane>()
            };

            var airplane = new Airplane()
            {
                Brand = brand,
                BrandId = brand.Id,
                Id = 1,
                AggregatedFlownDistance = 0,
                ProductionDate = new DateTime(1995, 1, 1)
            };

            brand.Airplanes.Add(airplane);

            airplaneRepo.Create(airplane);
            brandRepo.Create(brand);

            var sut = new AirplaneLogic(airplaneRepo, brandRepo);

            //Act
            var result = sut.GetAirplanesOf("kualalumpur");

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void GetAirplanesOf_WithMatchingBrand_ReturnsOneElement()
        {
            //Arrange
            var expectedBrandName = "KualaLumpur";
            var airplaneRepo = new InMemoryRepository<Airplane>();
            var brandRepo = new InMemoryRepository<Brand>();
            var brand = new Brand()
            {
                Id = 1,
                MaxFlightDistance = 3000,
                Name = expectedBrandName,
                NumberOfPassengerSeat = 200,
                Airplanes = new List<Airplane>()
            };

            var airplane = new Airplane()
            {
                Brand = brand,
                BrandId = brand.Id,
                Id = 1,
                AggregatedFlownDistance = 0,
                ProductionDate = new DateTime(1995, 1, 1)
            };

            brand.Airplanes.Add(airplane);

            airplaneRepo.Create(airplane);
            brandRepo.Create(brand);

            var sut = new AirplaneLogic(airplaneRepo, brandRepo);

            //Act
            var result = sut.GetAirplanesOf(expectedBrandName).ToList();

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Brand.Name, Is.EqualTo(expectedBrandName));
        }
        
        [TestCase(2)]
        [TestCase(100)]
        [TestCase(10000)]
        [TestCase(1000000)]
        public void GetAirplanesOf_WithMatchingBrand_ReturnsWithMultipleHit(int numberOfElements)
        {
            //Arrange
            var expectedBrandName = "KualaLumpur";
            var airplaneRepo = new InMemoryRepository<Airplane>();
            var brandRepo = new InMemoryRepository<Brand>();
            var brand = new Brand()
            {
                Id = 1,
                MaxFlightDistance = 3000,
                Name = expectedBrandName,
                NumberOfPassengerSeat = 200,
                Airplanes = new List<Airplane>()
            };

            var airplane = new Airplane()
            {
                Brand = brand,
                BrandId = brand.Id,
                Id = 1,
                AggregatedFlownDistance = 0,
                ProductionDate = new DateTime(1995, 1, 1)
            };

            brand.Airplanes.Add(airplane);

            for (int i = 0; i < numberOfElements; i++)
            {
                airplaneRepo.Create(airplane);
            }

            brandRepo.Create(brand);

            var sut = new AirplaneLogic(airplaneRepo, brandRepo);

            //Act
            var result = sut.GetAirplanesOf(expectedBrandName).ToList();

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(numberOfElements));
            foreach (var airplane1 in result)
            {
                Assert.That(airplane1.Brand.Name, Is.EqualTo(expectedBrandName));
            }
            
        }

        [Test]
        public void GetAirplanesOf_WithMultipleBrand_ReturnsOnlyMatchingElement()
        {
            //Arrange
            var expectedBrandName = "KualaLumpur";
            var airplaneRepo = new InMemoryRepository<Airplane>();
            var brandRepo = new InMemoryRepository<Brand>();
            var sut = new AirplaneLogic(airplaneRepo, brandRepo);

            // matching airplane
            var brand = new Brand()
            {
                Id = 1,
                MaxFlightDistance = 3000,
                Name = expectedBrandName,
                NumberOfPassengerSeat = 200,
                Airplanes = new List<Airplane>()
            };

            var airplane = new Airplane()
            {
                Brand = brand,
                BrandId = brand.Id,
                Id = 1,
                AggregatedFlownDistance = 0,
                ProductionDate = new DateTime(1995, 1, 1)
            };
            brand.Airplanes.Add(airplane);

            // not matching brand and airplane
            var notMatchingBrand = new Brand()
            {
                Id = 2,
                MaxFlightDistance = 2000,
                Name = expectedBrandName.ToLower(),
                NumberOfPassengerSeat = 200,
                Airplanes = new List<Airplane>()
            };

            var notMatchingAirplane = new Airplane()
            {
                Brand = notMatchingBrand,
                BrandId = notMatchingBrand.Id,
                Id = 2,
                AggregatedFlownDistance = 0,
                ProductionDate = new DateTime(1995, 1, 1)
            };

            notMatchingBrand.Airplanes.Add(notMatchingAirplane);

            // Add objects to Repo
            airplaneRepo.Create(airplane);
            airplaneRepo.Create(notMatchingAirplane);
            
            brandRepo.Create(brand);
            brandRepo.Create(notMatchingBrand);

            
            //Act
            var result = sut.GetAirplanesOf(expectedBrandName).ToList();

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Brand.Name, Is.EqualTo(expectedBrandName));
        }

        [Test]
        public void IncrementAggregatedDistance_WithExistingId_DistanceAggregated()
        {
            int res = int.MaxValue;
            var tmp = res + res;

            //Arrange
            int initialAggregatedFlownDistance = 200;
            int distanceToAdd = 1000;
            int expectedAggregatedFlownDistance = initialAggregatedFlownDistance + distanceToAdd;
            int airplaneId = 1;

            var airplaneRepo = new InMemoryRepository<Airplane>();
            var brandRepo = new InMemoryRepository<Brand>();
            var brand = new Brand()
            {
                Id = 1,
                MaxFlightDistance = 3000,
                Name = "KualaLumpur",
                NumberOfPassengerSeat = 200,
                Airplanes = new List<Airplane>()
            };

            var airplane = new Airplane()
            {
                Brand = brand,
                BrandId = brand.Id,
                Id = airplaneId,
                AggregatedFlownDistance = initialAggregatedFlownDistance,
                ProductionDate = new DateTime(1995, 1, 1)
            };

            brand.Airplanes.Add(airplane);

            airplaneRepo.Create(airplane);
            brandRepo.Create(brand);

            var sut = new AirplaneLogic(airplaneRepo, brandRepo);

            // Act
            var result = sut.IncrementAggregatedDistance(distanceToAdd, airplaneId);

            // Assert
            Assert.That(result, Is.EqualTo(true));

            Assert.That(airplane.AggregatedFlownDistance, Is.EqualTo(expectedAggregatedFlownDistance));

            var airplaneInRepo = airplaneRepo.GetOne(airplaneId);
            Assert.That(airplaneInRepo.AggregatedFlownDistance, Is.EqualTo(expectedAggregatedFlownDistance));
        }


    }
}