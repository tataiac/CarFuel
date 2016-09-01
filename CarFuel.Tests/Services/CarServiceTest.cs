using CarFuel.Models;
using CarFuel.Services;
using CarFuel.Tests.Fakes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Should;
using CarFuel.DataAccess.Bases;
using CarFuel.Services.Bases;
using Moq;

namespace CarFuel.Tests.Services {
  public class CarServiceTest {

    //public class AddMethod {

    //  private IRepository<Car> db;
    //  private IService<Car> s;

    //  public AddMethod() {
    //    db = new FakeRepository<Car>();
    //    s = new CarServices(db);
    //  }

    //  [Fact]
    //  public void AddFirstCar_Success() {
    //    var c = new Car("Toyota");
    //    s.Add(c);

    //    var cars = s.All();
    //    cars.Count().ShouldEqual(1);
    //  }

    //  [Fact]
    //  public void AddTwoCarsWithDuplicateName_ShouldNotBeAdded() {
    //    var c1 = new Car("Honda");
    //    var c2 = new Car("Honda");

    //    s.Add(c1);
    //    var ex = Assert.Throws<Exception>(() => {
    //      s.Add(c2);
    //    });

    //    ex.Message.ShouldEqual("This car name has been used already.");
    //    s.All().Count().ShouldEqual(1);
    //  }
    //}

    public class AddMethodWithMoq {

      [Fact]
      public void AddFirstCar_Success() {
        var user = new User { DisplayName = "User 1" };
        var mockUser = new Mock<IUserService>();
        mockUser.Setup(u => u.CurrentUser).Returns(user);

        var mock = new Mock<IRepository<Car>>();

        var s = new CarServices(mock.Object, mockUser.Object);

        //mock.Setup(m => m.Add(It.IsAny<Car>()));

        var c = new Car("Toyota");
        c.Owner = user;
        s.Add(c);

        //mock.Verify(r => r.Add(It.IsAny<Car>()), Times.Once);//Check repository was called only 1 time.
        mock.Verify(r => r.Add(c), Times.Once);//Check repository was called only 1 time with this parameter.
      }

      [Fact]
      public void AddTwoCarsWithDuplicateName_ShouldNotBeAdded() {
        var user = new User { DisplayName = "User 1" };
        var mockUser = new Mock<IUserService>();
        mockUser.Setup(u => u.CurrentUser).Returns(user);

        var mock = new Mock<IRepository<Car>>();

        var collection = new HashSet<Car>();
        mock.Setup(repo => repo.Add(It.IsAny<Car>())).Callback<Car>((c) => {
          collection.Add(c);
        });

        mock.Setup(repo => repo.Query(It.IsAny<Func<Car, bool>>())).Returns(collection.AsQueryable());

        var s = new CarServices(mock.Object, mockUser.Object);

        var c1 = new Car("Jazz") { Owner = user };
        var c2 = new Car("Jazz") { Owner = user };

        s.Add(c1);
        var ex = Assert.Throws<Exception>(() => {
          s.Add(c2);
        });

        ex.Message.ShouldEqual("This car name has been used already.");//False because mock wasn't keep value in HashSet
        mock.Verify(r => r.Add(c1), Times.Once);
      }
    }
  }

}
