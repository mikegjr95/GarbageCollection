using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Web.Mvc;
using TrashCollection.Models;


namespace TrashCollection.Controllers
{
    public class EmployeeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Employee
        public ActionResult Index(Customer customer)
        {
            {
                var Customer = db.Customers.Include(c => c.ApplicationUser);
                return View(Customer.ToList());
            }
        }

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
            ViewBag.ApplicationId = new SelectList(db.Users, "Id", "Email");
            return View();
        }

        // POST: Employee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ApplicationId,FirstName,LastName,ZipCode")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.ApplicationId = User.Identity.GetUserId();
                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index","Home");
            }

            ViewBag.ApplicationId = new SelectList(db.Users, "Id", "Email", employee.ApplicationId);
            return View(employee);
        }

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

        public ActionResult TodaysPickups(Employee employee)
        {
            var dateAndTime = DateTime.Now;
            var date = dateAndTime.Date;
            string currentDay = DateTime.Now.DayOfWeek.ToString();
            var todaysDate = date;
            var todaysWork = db.Customers.Where(c => c.PickupDay == currentDay && c.ZipCode == employee.ZipCode || c.ZipCode == employee.ZipCode && c.ExtraPickupDate == todaysDate).ToList();

            return View(todaysWork);
        }
    }
}
