using Lufthansa.Data;
using Microsoft.EntityFrameworkCore;

namespace Lufthansa.Repository;



public class BrandRepository : Repository<Brand>, IBrandRepository
{

    public BrandRepository(AirplaneDbContext context) : base(context)
    {
        
    }
    

} 