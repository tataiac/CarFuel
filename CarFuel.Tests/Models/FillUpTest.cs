using CarFuel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CarFuel.Tests.Models {
  public class FillUpTest {

    public class KmLProperty {//Nested Class (Inner Class)
      [Fact]
      public void NewFillUpDontHaveKmL() {
        // Arrange
        var f1 = new FillUp();

        // Act
        double? kml = f1.KmL;

        // Assert
        Assert.Null(kml);
      }

      [Fact]
      public void FillUpCalculateKmL_Case1() {
        // Arrange
        var f1 = new FillUp();
        f1.OdoMeter = 1000;
        f1.Liters = 40.0;
        f1.IsFull = true;

        var f2 = new FillUp();
        f2.OdoMeter = 2000;
        f2.Liters = 50.0;
        f2.IsFull = true;

        f1.NextFillUp = f2;

        // Act
        var kml = f1.KmL;

        // Assert
        Assert.Equal(20.0, kml);
      }

      [Fact]
      public void FillUpCalculateKmL_Case2() {
        // Arrange
        //var f1 = new FillUp();
        //f1.OdoMeter = 2000;
        //f1.Liters = 50.0;
        //f1.IsFull = true;

        //var f2 = new FillUp();
        //f2.OdoMeter = 2500;
        //f2.Liters = 20.0;
        //f2.IsFull = true;
        
        // Or
        //var f1 = new FillUp(odoMeter: 2000, liters: 50, isFull: true);
        //var f2 = new FillUp(2500, 20, true);

        // Or
        var f1 = new FillUp(odoMeter: 2000, liters: 50);
        var f2 = new FillUp(2500, 20);

        f1.NextFillUp = f2;

        // Act
        var kml = f1.KmL;

        // Assert
        Assert.Equal(25.0, kml);
      }

      [Theory]// Choose between theory of fact
      [InlineData(1000, 40.0, 2000, 50.0, 20.0)]//Case 1
      [InlineData(2000, 50.0, 2500, 20.0, 25.0)]//Case 2
      public void FillUpCalculateKmL(int odo1, double liters1, int odo2, double liters2, double expectedKmL) {
        var f1 = new FillUp(odoMeter: odo1, liters: liters1);
        var f2 = new FillUp(odo2, liters2);

        f1.NextFillUp = f2;

        // Act
        var kml1 = f1.KmL;
        var kml2 = f2.KmL;

        // Assert
        Assert.Equal(expectedKmL, kml1);
        Assert.Null(kml2);
      }


      [Fact]
      public void OdoMeterMustGreaterThanThePreviousFillUp() {
        var f1 = new FillUp(50000, 50);
        var f2 = new FillUp(49000, 60);
        f1.NextFillUp = f2;

        var ex = Assert.Throws <Exception> (() => {
          var kml1 = f1.KmL;
        });

        Assert.Equal("Invalid OdoMeter Value.", ex.Message);

      }
    }

  }
}
