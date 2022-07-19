using Lufthansa.Data;
using Lufthansa.Logic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Lufthansa.Endpoint.Controllers
{
    [Route("[controller]")]
    [Route("api/[controller]/api")]
    [Route("")]
    [ApiController]
    public class AirplaneController : ControllerBase
    {
        private readonly IAirplaneLogic _logic;

        public AirplaneController(IAirplaneLogic logic)
        {
            _logic = logic;
        }

        // GET: api/<AirplaneController>
        [HttpGet]
        public IEnumerable<Airplane> GetAll()
        {
            return _logic.GetAllAirplane().Select(airplane =>
            {
                airplane.Brand = default(Brand);
                return airplane;
            });
        }

        [HttpGet("pure")]
        public IEnumerable<Airplane> GetAllPure()
        {
            return _logic.GetAllAirplane().Select(airplane =>
            {
                airplane.Brand.Airplanes = null;
                return airplane;
            });
        }

        // GET api/<AirplaneController>/5
        [HttpGet("{id}")]
        public Airplane Get(int id)
        {
            return _logic.GetOneAirplane(id);
        }

        // POST api/<AirplaneController>
        [HttpPost]
        public void Post([FromBody] Airplane value)
        {
            _logic.CreateAirplane(value);
        }

        // PUT api/<AirplaneController>/5
        
        [HttpPut("{id}")]
        public string Put(int id, [FromBody] Airplane value)
        {
            try
            {
                var airplane = value;
                //var airplane = _logic.GetAirplaneById(id);
                //airplane.AggregatedFlownDistance = value.AggregatedFlownDistance;
                //airplane.ProductionDate = value.ProductionDate;

                _logic.UpdateAirplane(id, airplane);

                return "Success";
            }
            catch (Exception e)
            {
                return "Error";
            }
        }

        // DELETE api/<AirplaneController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _logic.DeleteAirplaneById(id);
        }

        public class AirplaneUpdate
        {
            /// <summary>
            /// TODO: refactor to DateOnly
            /// </summary>
            public DateTime ProductionDate { get; set; }

            /// <summary>
            /// Distance flown in the whole life of the aircraft in km
            /// </summary>
            public int? AggregatedFlownDistance { get; set; }
        }
    }
}
