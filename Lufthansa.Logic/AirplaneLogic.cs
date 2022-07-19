using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Lufthansa.Data;
using Lufthansa.Repository;
using Microsoft.EntityFrameworkCore;

namespace Lufthansa.Logic
{

    public interface IAirplaneLogic
    {
        IRepository<Airplane> GetAirplaneRepository();

        IEnumerable<Airplane> GetAllAirplane();

        void CreateAirplane(Airplane airplane);

        bool IncrementAggregatedDistance(int distanceToAdd, int airplaneId);

        void CreateTestData();
        IEnumerable<Brand> GetAllBrands();

        // Sample for default interface implementation
        Airplane GetAirplaneById(int id)
        {
            return new Airplane().CopyFrom(GetAirplaneRepository().GetOne(id));
        }

        Airplane GetOneAirplane(int id);

        void DeleteAirplaneById(int id);

        void UpdateAirplane(int id, Airplane airplane);
    }


    public class AirplaneLogic : IAirplaneLogic
    {
        private readonly IRepository<Airplane> _repo;
        private readonly IRepository<Brand> _brandRepo;
        private IAirplaneLogic _airplaneLogicImplementation;

        public AirplaneLogic(IRepository<Airplane> repo, IRepository<Brand> brandRepo)
        {
            if (repo == null )
            {
                throw new ArgumentNullException(nameof(repo));
            }

            if (brandRepo == null)
            {
                throw new ArgumentNullException(nameof(brandRepo));
            }
            _repo = repo;
            _brandRepo = brandRepo;
        }

        public IEnumerable<Airplane> GetAirplanesOf(string brandName)
        {
            return _repo.GetAll()
                .Where(_ => _.Brand.Name == brandName);
        }


        public int GetTotalAggregatedFlownDistanceFor(string brandName)
        {
            var retVal = _repo.GetAll()
                .Where(_ => _.Brand.Name == brandName)
                .Sum(_ => _.AggregatedFlownDistance);

            if (!retVal.HasValue)
            {
                return 0;
            }

            return retVal.Value;
        }

        public int GetTotalAggregatedFlownDistanceFor(Brand brand)
        {
            return GetTotalAggregatedFlownDistanceFor(brand.Name);
        }

        public IEnumerable<Tuple<string, int>> GetTotalAggregatedFlownDistanceForAllBrands()
        {
            var retVal = _repo.GetAll().ToList()
                .GroupBy(_ => _.Brand.Id)
                .Select((group, _) =>
                    new Tuple<string, int> (group.First().Brand.Name, 
                        (int)group.Sum(_ => _.AggregatedFlownDistance))
                 );
            return retVal.ToList();
        }

        public bool CanFlyForDistance(int distance, string brandName)
        {
            return _brandRepo
                .GetAll()
                .Any(_ => _.MaxFlightDistance >= distance && _.Name == brandName);
        }

        public IEnumerable<Brand> GetBrandsWhichCanFlyFor(int distance)
        {
            return _brandRepo
                .GetAll()
                .Where(_ => _.MaxFlightDistance >= distance);
        }

        public bool IncrementAggregatedDistance(int distanceToAdd, int airplaneId)
        {
            if (distanceToAdd < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(distanceToAdd), distanceToAdd,"Argument must be greater than 0");
            }

            using (var tr = new TransactionScope())
            {
                var airplaneToModify = _repo.GetOne(airplaneId);
                if (airplaneToModify == null)
                {
                    return false;
                }

                if (!airplaneToModify.AggregatedFlownDistance.CanAdd(distanceToAdd))
                {
                    throw new OverflowException();
                }

                var modifiedAirplane = airplaneToModify;
                modifiedAirplane.AggregatedFlownDistance += distanceToAdd;

                _repo.Update(airplaneId, modifiedAirplane);
                tr.Complete();
            }

            return true;
            
        }

        public IRepository<Airplane> GetAirplaneRepository()
        {
            return _repo;
        }

        public IEnumerable<Airplane> GetAllAirplane()
        {
            return _repo.GetAll().ToList().Select(_ => new Airplane().CopyFrom(_));
        }

        public void CreateAirplane(Airplane airplane)
        {
            _repo.Create(airplane);
        }

        public void CreateTestData()
        {
            if (_repo is not InMemoryRepository<Airplane>)
            {
                return;
            }

            var brand1 = new Brand()
            {
                Id = 1,
                MaxFlightDistance = 1000,
                NumberOfPassengerSeat = 120,
                Name = "Boeing 747C",
                Airplanes = new List<Airplane>()
            };

            var brand2 = new Brand()
            {
                Id = 2,
                MaxFlightDistance = 1500,
                NumberOfPassengerSeat = 80,
                Name = "Airbus A320",
                Airplanes = new List<Airplane>()
            };

            var airplane = new Airplane()
            {
                Id = 1,
                AggregatedFlownDistance = 0,
                ProductionDate = new DateTime(2000, 1, 1),
                BrandId = 1,
                Brand = brand1,
            };

            brand1.Airplanes.Add(airplane);

            var airplane2 = new Airplane()
            {
                Id = 2,
                AggregatedFlownDistance = 0,
                ProductionDate = new DateTime(2004, 1, 1),
                BrandId = 2,
                Brand = brand2,

            };

            brand2.Airplanes.Add(airplane2);

            var airplane3 = new Airplane()
            {
                Id = 3,
                AggregatedFlownDistance = 0,
                ProductionDate = new DateTime(2005, 1, 1),
                BrandId = 2,
                Brand = brand2,
            };

            brand2.Airplanes.Add(airplane2);

            _repo.Create(airplane);
            _repo.Create(airplane2);
            _repo.Create(airplane3);

            _brandRepo.Create(brand1);
            _brandRepo.Create(brand2);
        }

        public IEnumerable<Brand> GetAllBrands()
        {
            return _brandRepo.GetAll().ToList().Select(_ => new Brand().CopyFrom(_));
        }

        public Airplane GetOneAirplane(int id)
        {
            return _repo.GetOne(id);
        }

        public void DeleteAirplaneById(int id)
        {
            _repo.Delete(id);
        }

        public void UpdateAirplane(int id, Airplane airplane)
        {
            _repo.Update(id, airplane);
        }
    }

    public static class asdgfafdsg
    {
        public static bool CanAdd(this int? a, int? b)
        {
            if (a == null || b == null)
            {
                return true;
            }

            return int.MaxValue - a >= b;
        }
    }
}
