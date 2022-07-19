using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lufthansa.Data;
using Lufthansa.Repository;
using Moq;

namespace Lufthansa.Logic.Tests
{
    public partial class AirplaneLogicTests
    {
        [Test]
        public void IncrementAggregatedDistance_WithMoqAndExistingId_DistanceAggregated()
        {
            // Arrange
            int initialAggregatedFlownDistance = 200;
            int distanceToAdd = 1000;
            int expectedAggregatedFlownDistance = initialAggregatedFlownDistance + distanceToAdd;
            int airplaneId = 1;

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

            Mock<IRepository<Brand>> brandRepoMock = new Mock<IRepository<Brand>>();
            Mock<IRepository<Airplane>> airplaneRepositoryMock = new Mock<IRepository<Airplane>>();
            airplaneRepositoryMock
                .Setup(_ => _.GetOne(It.IsAny<int>()))
                .Returns(airplane);
            
            var sut = new AirplaneLogic(airplaneRepositoryMock.Object, brandRepoMock.Object);
            var numberOfUpdateCalls = 0;
            var isIdCorrectInUpdateCall = false;

            airplaneRepositoryMock.Setup(_ => _.Update(It.IsAny<int>(), It.IsAny<Airplane>()))
                .Callback((int id, Airplane plane) =>
                {
                    numberOfUpdateCalls++;
                    isIdCorrectInUpdateCall = id == airplaneId;
                });

            //airplaneRepositoryMock.Setup(_ => _.Update(It.IsAny<int>(), It.IsAny<Airplane>()))
            //      .Throws(new ArgumentNullException());

            //Act
            var result = sut.IncrementAggregatedDistance(distanceToAdd, airplaneId);

            //Assert
            airplaneRepositoryMock.Verify(_ => _.GetOne(It.IsAny<int>()), 
                Times.AtLeast(1));
            airplaneRepositoryMock.Verify(_ => _.Update(It.IsAny<int>(), It.IsAny<Airplane>()), 
                Times.Exactly(1));
            Assert.That(numberOfUpdateCalls, Is.EqualTo(1));
            Assert.That(isIdCorrectInUpdateCall, Is.EqualTo(true));

            airplaneRepositoryMock.Verify(_ => _.GetOne(It.Is<int>(id => id == airplaneId)));
            airplaneRepositoryMock.Verify(_ => _.Update(It.Is<int>(id => id == airplaneId), It.Is<Airplane>(plane => plane.AggregatedFlownDistance == expectedAggregatedFlownDistance)));
            
        }

        [Test]
        public void IncrementAggregatedDistance_WithIntMaxValue_DistanceAggregated()
        {
            // Arrange
            int initialAggregatedFlownDistance = 200;
            int distanceToAdd = int.MaxValue;
            //int expectedAggregatedFlownDistance = initialAggregatedFlownDistance + distanceToAdd;
            int airplaneId = 1;

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

            Mock<IRepository<Brand>> brandRepoMock = new Mock<IRepository<Brand>>();
            Mock<IRepository<Airplane>> airplaneRepositoryMock = new Mock<IRepository<Airplane>>();
            airplaneRepositoryMock
                .Setup(_ => _.GetOne(It.IsAny<int>()))
                .Returns(airplane);

            var sut = new AirplaneLogic(airplaneRepositoryMock.Object, brandRepoMock.Object);
            var numberOfUpdateCalls = 0;
            var isIdCorrectInUpdateCall = false;
            
            airplaneRepositoryMock.Setup(_ => _.Update(It.IsAny<int>(), It.IsAny<Airplane>()))
                .Callback((int id, Airplane plane)=>
                {
                    numberOfUpdateCalls++;
                    isIdCorrectInUpdateCall = id == airplaneId;
                });

            //airplaneRepositoryMock.Setup(_ => _.Update(It.IsAny<int>(), It.IsAny<Airplane>()))
            //      .Throws(new ArgumentNullException());

            //Act
            var result = Assert.Throws<OverflowException>(()=>sut.IncrementAggregatedDistance(distanceToAdd, airplaneId));

            //Assert
            airplaneRepositoryMock.Verify(_ => _.GetOne(It.IsAny<int>()),
                Times.AtLeast(1));

            airplaneRepositoryMock.Verify(_ => _.GetOne(It.Is<int>(id => id == airplaneId)));
            //airplaneRepositoryMock.Verify(_ => _.Update(It.Is<int>(id => id == airplaneId), It.Is<Airplane>(plane => plane.AggregatedFlownDistance == expectedAggregatedFlownDistance)));

        }

    }
}
