using Microsoft.EntityFrameworkCore;

namespace Lufthansa.Data;

public interface IDbContext<T>
    where T : class
{
    DbSet<T> GetDbSet();
    int SaveChanges();
    IQueryable<T> GetDbSetWithInclude();
}