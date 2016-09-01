using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CarFuel.Models {
  public class Car {

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public string Name { get; set; } //EF wanna use property so we need to replace private variable to property.
    public virtual ICollection<FillUp> FillUps { get; set; } = new HashSet<FillUp>();//C# 6.0 Command or use at all constructor(s).

    [Required]
    public virtual User Owner { get; set; }

    public Car() : this("Car") {
      //Default if you don't use other constructor, compiler will auto create blank ctor while compiling.
      // : this("Car") > For use ctro that has more parameter.
    }

    public Car(string name) {
      Name = name;
    }

    public FillUp AddFillUp(int odoMeter, double liters) {
      FillUp f = new FillUp(odoMeter, liters);

      //var prevFillUp = FillUps.LastOrDefault();
      var prevFillUp = FillUps.Where(x => x.NextFillUp == null).FirstOrDefault();
      if (prevFillUp != null) prevFillUp.NextFillUp = f;

      FillUps.Add(f);

      return f;
    }

    public double? AverageKmL {
      get {
        if (FillUps.Count <= 1) return null;

        int deltaKM = FillUps.Last().OdoMeter - FillUps.First().OdoMeter;
        double sumLiters = FillUps.Skip(1).Sum(x => x.Liters);

        return Math.Round(deltaKM / sumLiters, 2, MidpointRounding.AwayFromZero);
        //MidpointRounding.AwayFromZero 5 Always Up
      }
    }

  }
}