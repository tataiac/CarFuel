using System;
using System.Linq;

namespace CarFuel.Services.Bases {
  public interface IService<T> where T : class {

    T Find(params object[] keys);

    IQueryable<T> All();
    IQueryable<T> Query(Func<T, bool> criteria);
    T Add(T item);
    T Remove(T item);

    int SaveChanges();
  }
}
