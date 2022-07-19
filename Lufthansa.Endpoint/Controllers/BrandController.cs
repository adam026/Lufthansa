using Lufthansa.Data;
using Lufthansa.Logic;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Lufthansa.Endpoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {

        private readonly IAirplaneLogic _logic;

        public BrandController(IAirplaneLogic logic)
        {
            _logic = logic;
        }
        // GET: api/<BrandController>/copy/
        [HttpGet]
        public IEnumerable<Brand> GetAll()
        {
            return _logic.GetAllBrands().Select(brand =>
            {
                foreach (var airplane in brand.Airplanes)
                {
                    airplane.Brand = null;
                }
                return brand;
            });
        }

        // GET api/<BrandController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<BrandController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<BrandController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<BrandController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
