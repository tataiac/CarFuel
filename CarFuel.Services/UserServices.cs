using CarFuel.DataAccess.Bases;
using CarFuel.Models;
using CarFuel.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarFuel.Services {
  public class UserServices : ServiceBase<User>, IUserService {

    public UserServices(IRepository<User> baseRepo) : base(baseRepo) {

    }

    private User _currentUser;
    public User CurrentUser {
      get { return _currentUser; }
      set { _currentUser = value; }
    }

    public override User Find(params object[] keys) {//Parameter for many parameters
      var key1 = (Guid)keys[0];
      return Query(x => x.UserId == key1).SingleOrDefault();
    }

  }
}
