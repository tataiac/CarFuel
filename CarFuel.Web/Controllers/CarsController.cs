using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarFuel.Models;
using CarFuel.Web.Models;
using CarFuel.Services.Bases;
using Microsoft.AspNet.Identity;
using CarFuel.Services;

namespace CarFuel.Web.Controllers {
  //public class CarsController : Controller {
  public class CarsController : AppControllerBase {

    //private CarFuelDb db = new CarFuelDb();//This concept view can use this connection, so return to view don't require to ToList()

    //protected override void Dispose(bool disposing) {
    //  if (disposing) db.Dispose();//Every object has method dispose

    //  base.Dispose(disposing);
    //}

    //private readonly IService<Car> _carService;
    //public CarsController(IService<Car> carService) {
    private readonly ICarService _carService;

    //public CarsController(ICarService carService, IService<User> userService) {
    public CarsController(ICarService carService, IUserService userService) : base(userService) {//Call parent constructure before local constructure
      _carService = carService;
      //_userService = userService;
    }

    private Guid GetCurrentUser() {
      return new Guid(User.Identity.GetUserId());
    }

    public ActionResult Index() {
      //var query = from c in db.Cars
      //            select c;

      //return View(query);

      //return View(_carService.All());

      if (User.Identity.IsAuthenticated) {
        ViewBag.AppUser = _userService.CurrentUser;

        return View("IndexForMember", _carService.All());
        //return View("IndexForMember", _carService.AllCarsForMember(GetCurrentUser()));
      }
      else {
        return View("IndexForAnonymous");
      }
    }

    public ActionResult Add() {
      return View();
    }

    [HttpPost]
    public ActionResult Add(Car item) {
      ModelState.Remove("Owner");

      if (ModelState.IsValid) {
        User u = _userService.Find(GetCurrentUser());
        item.Owner = u;

        //db.Cars.Add(item);
        //db.SaveChanges();

        _carService.Add(item);
        _carService.SaveChanges();

        return RedirectToAction("Index");
      }

      return View(item);
    }

    public ActionResult AddFillUp(Guid id) {
      //var c = db.Cars.Find(id);//If not found => null
      //if (c == null) return HttpNotFound();

      //ViewBag.CarId = id;
      //ViewBag.CarName = (from c in db.Cars
      //                   where c.Id == id
      //                   select c.Name).SingleOrDefault();

      ViewBag.CarName = (from c in _carService.All()
                         where c.Id == id
                         select c.Name).SingleOrDefault();

      return View();
    }

    [HttpPost]
    public ActionResult AddFillUp(Guid id, [Bind(Exclude = "Id")] FillUp item) {
      //public ActionResult AddFillUp(Guid id, [Bind(Exclude = "Id")] FillUp item) { is black list field Id
      //or
      ModelState.Remove("Id");

      if (ModelState.IsValid) {

        //var car = db.Cars.Find(id);
        //db.Entry(car).Collection(x => x.FillUps).Load();// Manual Loading
        //db.Entry(fill).Reference(x => x.NextFillUp).Load();// In case of next fill up is not virtual.
        var car = _carService.Find(id);

        car.AddFillUp(item.OdoMeter, item.Liters);
        //db.SaveChanges();
        _carService.SaveChanges();

        return RedirectToAction("Index");
      }

      return View(item);
    }
  }
}
