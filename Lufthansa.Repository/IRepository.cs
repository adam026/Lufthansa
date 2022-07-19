using Lufthansa.Data;

namespace Lufthansa.Repository;

public interface IRepository<T>
{
    IQueryable<T> GetAll();
    T GetOne(int id);
    void Create(T item);
    void Delete(int id);
    void Update(int id, T targetValue);
}

public interface IAirplaneRepository : IRepository<Airplane>
{

}

public interface IBrandRepository : IRepository<Brand>
{

}