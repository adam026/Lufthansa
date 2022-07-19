using Lufthansa.Data;
using Microsoft.EntityFrameworkCore;

namespace Lufthansa.Repository
{
    

    public class AirplaneRepository : Repository<Airplane>, IAirplaneRepository
    {
        public AirplaneRepository(IDbContext<Airplane> context) : base(context)
        {
            
        }

        
    }
}
