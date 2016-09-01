using CarFuel.DataAccess.Bases;
using CarFuel.Models;
using CarFuel.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarFuel.Services {
  public class CarServices : ServiceBase<Car>, ICarService {

    private readonly IUserService _userService;

    public CarServices(IRepository<Car> baseRepo, IUserService userService) : base(baseRepo) {
      _userService = userService;
    }

    public override Car Find(params object[] keys) {//Parameter for many parameters
      var key1 = (Guid)keys[0];
      return Query(x => x.Id == key1).SingleOrDefault();
    }

    public override IQueryable<Car> All() {
      if (_userService.CurrentUser == null) {
        //If you want to return null please use this statement instead
        //return Enumerable.Empty<Car>().AsQueryable();
        throw new Exception("Please login before get car list !");
      }

      return Query(c => c.Owner == _userService.CurrentUser);
    }

    public override Car Add(Car item) {
      ValidateBeforeSave(item);

      return base.Add(item);
    }

    private bool ValidateBeforeSave(Car item) {
      if (All().Any(c => c.Name == item.Name)) {
        throw new Exception("This car name has been used already.");
      }

      return true;
    }

    //public IQueryable<Car> AllCarsForMember(Guid memberId) {
    //  return Query(c => c.Owner.UserId == memberId);
    //}
  }
}
