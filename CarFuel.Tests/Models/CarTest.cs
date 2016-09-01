using CarFuel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Should;

namespace CarFuel.Tests.Models {
  public class CarTest {

    public class General {
      [Fact]
      public void InitialValues() {
        var c = new Car(name: "My Jazz");

        Assert.Equal("My Jazz", c.Name);
        Assert.Empty(c.FillUps);

        c.Name.ShouldEqual("My Jazz");
        c.FillUps.ShouldBeEmpty();
      }
    }

    public class AddFillUpMethod {
      [Fact]
      public void AddFirstFillUp() {
        var c = new Car("My Ford");
        FillUp f = c.AddFillUp(odoMeter: 1000, liters: 40);

        Assert.Equal(1, c.FillUps.Count);
        Assert.Equal(1000, f.OdoMeter);
        Assert.Equal(40.0, f.Liters);
      }

      [Fact]
      public void AddTwoFillUpsAndThemShouldBeChainedCorrectly() {
        var c = new Car("My Ford");
        FillUp f1 = c.AddFillUp(odoMeter: 1000, liters: 40);
        FillUp f2 = c.AddFillUp(2000, 50);

        Dump(c);

        //Assert.Same(f2, f1.NextFillUp);//Equal is same value, Same is same object
        f1.NextFillUp.ShouldBeSameAs(f2);
      }

      //[Theory]
      //[MemberData("RandomFillUpData", 50)]// Test case will generate to 50 cases if preEnumerateTheories is true, false is the same test case but retry test 50 times.
      //public void AddSeveralFillUps(int odoMeter, double liters) {
      //  var c = new Car("Vios");
      //  c.AddFillUp(odoMeter, liters);
      //  c.FillUps.Count.ShouldEqual(1);
      //}

      public static IEnumerable<object[]> RandomFillUpData(int cnt) {
        //static is required and return must be IEnumerable
        //Parameter (cnt) for setup preEnumerateTheories by get all datas before run all cases.

        var r = new Random();

        for (int i = 0; i < cnt; i++) {
          int odo = r.Next(0, 999999 + 1);
          double liters = r.Next(0, 9999 + 1) / 10.0;
          yield return new object [] { odo, liters };
        }
      }

      private void Dump(Car c) {
        _output.WriteLine("Car: {0}", c.Name);
        foreach (var f in c.FillUps) {
          _output.WriteLine($"{f.OdoMeter:000000} {f.Liters:n2} L. {f.KmL:n2} Km/L.");
        }
      }

      private readonly ITestOutputHelper _output;

      public AddFillUpMethod(ITestOutputHelper output) {
        _output = output;
      }

    }

    public class AverageKmL {
      [Fact]
      public void GetAvgKmLFromNewCar() {
        var c = new Car("Toyota");

        Assert.Null(c.AverageKmL);
      }

      [Fact]
      public void GetAvgKmLFromOneFillUp() {
        var c = new Car("Toyota");

        c.AddFillUp(1000, 40);

        Assert.Null(c.AverageKmL);
      }

      [Fact]
      public void GetAvgKmLFromThreeFillUp() {
        var c = new Car("Honda");

        c.AddFillUp(1000, 40);
        c.AddFillUp(2000, 50);
        c.AddFillUp(2500, 20);

        Assert.Equal(21.43, c.AverageKmL);
      }

    }
  }
}
