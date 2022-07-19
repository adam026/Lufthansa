using Lufthansa.Data;
using Microsoft.EntityFrameworkCore;

namespace Lufthansa.Repository;

public class Repository<T> : IRepository<T>
    where T : class, IDbItem, ICopyFrom<T>, new()
{
    protected IDbContext<T> _context;

    public Repository(IDbContext<T> context)
    {
        _context = context;
    }

    public IQueryable<T> GetAll()
    {
        return _context.GetDbSetWithInclude();
    }

    public T GetOne(int id)
    {
        return GetAll().FirstOrDefault(_ => _.Id == id);
    }

    public void Create(T airplane)
    {
        _context.GetDbSet().Add(airplane);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var item = GetOne(id);
        _context.GetDbSet().Remove(item);
        _context.SaveChanges();
    }

    public void Update(int id, T targetValue)
    {
        var item = GetOne(id);
        item.CopyFrom(targetValue);
        item.Id = id;
        _context.SaveChanges();
    }
}