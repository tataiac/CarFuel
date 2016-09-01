namespace CarFuel.DataAccess.Migrations {
  using System;
  using System.Data.Entity.Migrations;

  public partial class Carownerisrequired : DbMigration {
    public override void Up() {
      DropForeignKey("dbo.Cars", "Owner_UserId", "dbo.Users");
      DropIndex("dbo.Cars", new[] { "Owner_UserId" });

      var zeroId = new Guid();
      Sql($"Update Cars Set Owner_UserId = '{zeroId}' Where Owner_UserId Is Null");

      AlterColumn("dbo.Cars", "Owner_UserId", c => c.Guid(nullable: false));
      CreateIndex("dbo.Cars", "Owner_UserId");
      AddForeignKey("dbo.Cars", "Owner_UserId", "dbo.Users", "UserId", cascadeDelete: true);
    }

    public override void Down() {
      DropForeignKey("dbo.Cars", "Owner_UserId", "dbo.Users");
      DropIndex("dbo.Cars", new[] { "Owner_UserId" });
      AlterColumn("dbo.Cars", "Owner_UserId", c => c.Guid());
      CreateIndex("dbo.Cars", "Owner_UserId");
      AddForeignKey("dbo.Cars", "Owner_UserId", "dbo.Users", "UserId");
    }
  }
}
