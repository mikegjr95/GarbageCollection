using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Contexts;
using System.Web.Mvc;
using TrashCollection.Models;



namespace TrashCollection.Controllers
{
    public class EmployeeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Employee
        public ViewResult Index(string sortOrder, string searchString)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            var customers = from s in db.Customers
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                customers = customers.Where(s => s.LastName.Contains(searchString)
                                       || s.PickupDay.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    customers = customers.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    customers = customers.OrderBy(s => s.PickupDay);
                    break;
                case "date_desc":
                    customers = customers.OrderByDescending(s => s.PickupDay);
                    break;
                default:
                    customers = customers.OrderBy(s => s.LastName);
                    break;
            }
            return View(customers.ToList());
        }
        //public ActionResult Index( Employee employee)
        //{
        //    {
        //        var Customer = db.Customers.Include(c => c.ApplicationUser);
        //        return View(Customer.ToList());
        //    }
        //}

        // GET: Employee/Details/5
        //        public ActionResult Details(int? id)
        public ActionResult Details(int? id)
        {
            Employee employee = new Employee();
            if (id == null)
            {
                var personId = User.Identity.GetUserId();
                employee = db.Employees.Where(p => p.ApplicationId == personId).Select(p => p).SingleOrDefault();
            }
            else
            {
                employee = db.Employees.Find(id);
            }

            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }  
            
            
 //       {
 //           if (id == null)
 //           {
 //               return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
 //          }
//            Employee employee = db.Employees.Find(id);
//            if (employee == null)
//            {
 //               return HttpNotFound();
//            }
 //           return View(employee);
 //       }

        // GET: Employee/Create
        public ActionResult Create()
        {
            Employee employee = new Employee();
            return View(employee);
        }

        // POST: Employee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(/*[Bind(Include = "Id,ApplicationId,FirstName,LastName,ZipCode")]*/ Employee employee)

        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                employee.ApplicationId = userId;
                var foundEmployee = db.Users.Where(e => e.Id == employee.ApplicationId).FirstOrDefault();
                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
        //{
        //    if (ModelState.IsValid)
        //    {
        //        employee.ApplicationId = User.Identity.GetUserId();
        //        db.Employees.Add(employee);
        //        db.SaveChanges();
        //        return RedirectToAction("Index","Home");
        //    }

        //    ViewBag.ApplicationId = new SelectList(db.Users, "Id", "Email", employee.ApplicationId);
        //    return View(employee);
        //}

        // GET: Employee/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApplicationId = new SelectList(db.Users, "Id", "Email", employee.ApplicationId);
            return View(employee);
        }

        // POST: Employee/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ApplicationId,FirstName,LastName,ZipCode")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApplicationId = new SelectList(db.Users, "Id", "Email", employee.ApplicationId);
            return View(employee);
        }

        // GET: Employee/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]

        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee );
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult TodaysPickups(Employee employee,Customer customer, string id)
        {

            var dateAndTime = DateTime.Now;
            var date = dateAndTime.Date;
            string currentDay = DateTime.Now.DayOfWeek.ToString();
            var todaysDate = date;
            var userId = User.Identity.GetUserId();
            var employees = db.Employees.Where(c => c.ApplicationId == userId).FirstOrDefault();
            int areaCode = employees.ZipCode;
            var todaysWork = db.Customers.Where(c => c.PickupDay == currentDay && c.ZipCode == areaCode || c.ZipCode == areaCode && c.ExtraPickupDate == todaysDate).ToList();

            return View(todaysWork);
        }

        public ActionResult CustomerLocation(int id, Customer customer, string customer1)
        {
            var customerDetails = db.Customers.Where(a => a.Id == id).FirstOrDefault();
            customer.apiMapCall = PrivateKeys.googleMap;
            db.SaveChanges();
            if (User.IsInRole("Employee"))
            {
                ViewBag.CID = customer1;
            }
            return View(customerDetails);
        }



        //public async System.Threading.Tasks.Task<Customer> GetLatNLngAsync(Customer customer)
        //{
        //    var e = customer;
        //    string url = PrivateKeys.geoURLP1 + e.StreetAddress + ",+" + e.City + "+" + e.State + PrivateKeys.geoURLP2 + PrivateKeys.googleKey;
        //    HttpClient client = new HttpClient();
        //    HttpResponseMessage response = await client.GetAsync(url);
        //    string jsonResult = await response.Content.ReadAsStringAsync();
        //    if (response.IsSuccessStatusCode)
        //    {
        //        GeoCode location = JsonConvert.DeserializeObject<GeoCode>(jsonResult);
        //        e.Lat = location.results[0].geometry.location.lat;
        //        e.Lng = location.results[0].geometry.location.lng;
        //        return e;
        //    }

        //    db.Customers.Add(e);
        //    db.SaveChanges();
        //    return e;
        //}
    }
}
