using System.Data.Entity;
using CarFuel.Models;

namespace CarFuel.DataAccess.Contexts {
  public class CarFuelDb : DbContext {

    public DbSet<User> Users { get; set; }
    public DbSet<Car> Cars { get; set; }
    //public DbSet<FillUp> FillUps { get; set; }
  }
}