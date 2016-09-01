using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarFuel.Models {

  [Table("tblFillUp")]
  public class FillUp {

    #region Property
    //In order to use key by not using Id or <Class>.Id
    //Id by explicit
    //[Key]
    //public int FillUpId { get; set; }
    public int Id { get; set; }//Id by convension

    [Range(0, 999999)]
    public int OdoMeter { get; set; }

    [Range(0.0, 100.0)]
    public double Liters { get; set; }

    [Column("IS_FULL")]
    public bool IsFull { get; set; }

    //Navigation Property
    //Recomended lazy-loading by using virtual
    public virtual FillUp NextFillUp { get; set; }
    #endregion

    #region Constructor
    public FillUp() {
    }

    public FillUp(int odoMeter, double liters, bool isFull = true) {
      OdoMeter = odoMeter;
      Liters = liters;
      IsFull = isFull;
    }
    #endregion

    public double? KmL {
      get {
        if (NextFillUp == null) return null;
        if (NextFillUp.OdoMeter < OdoMeter) throw new Exception("Invalid OdoMeter Value.");

        return (NextFillUp.OdoMeter - OdoMeter) / NextFillUp.Liters;
      }
    }
    
  }
}