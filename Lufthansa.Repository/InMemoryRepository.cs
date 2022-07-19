using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lufthansa.Data;

namespace Lufthansa.Repository
{
    public class InMemoryRepository<T> : IRepository<T>
        where T : class, IDbItem, ICopyFrom<T>, new()
    {
        public InMemoryRepository()
        {
            _list = new List<T>();
        }

        List<T> _list;
        public IQueryable<T> GetAll()
        {
            return _list.Select(_ => new T().CopyFrom(_)).AsQueryable();
        }

        public T GetOne(int id)
        {
            var item =  _list.FirstOrDefault(_ => _.Id==id);
            if (item == null)
            {
                return null;
            }
            return new T().CopyFrom(item);
        }

        public void Create(T item)
        {
            _list.Add(item);
        }

        public void Delete(int id)
        {
            var item = GetOne(id);
            if (item == null)
            {
                return;
            }
            _list.Remove(item);
        }

        public void Update(int id, T targetValue)
        {
            var itemToUpdate = _list.SingleOrDefault(_ => _.Id == id);
            if (itemToUpdate == null)
            {
                throw new ArgumentException("argument value was not found in Database", nameof(id));
            }

            itemToUpdate.CopyFrom(targetValue);
        }
    }

}
