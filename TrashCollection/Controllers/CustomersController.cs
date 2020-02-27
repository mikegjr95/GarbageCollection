using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TrashCollection.Models;

namespace TrashCollection.Controllers
{
    public class CustomersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //GET: Customers
        public ActionResult Index()
        {
            var personId = User.Identity.GetUserId();
            var customer = db.Customers.Where(p => p.ApplicationId == personId).Select(p => p).ToList();
            return View(customer);

        }

        // GET: Customers/Details/5
        public ActionResult Details(string id)
        {
            string userId = User.Identity.GetUserId();
            var customer = db.Customers.Where(u => u.ApplicationId == userId).FirstOrDefault();

                return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            Customer customer = new Customer();
            return View(customer);
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.ExtraPickupDate = DateTime.Now;
                customer.SuspendedStart = DateTime.Now;
                customer.SuspendedEnd = DateTime.Now;
                customer.ApplicationId = User.Identity.GetUserId();
                customer = await GetLatNLngAsync(customer);
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index","Home");
            }

            ViewBag.ApplicationId = new SelectList(db.Users, "Id", "Email", customer.ApplicationId);
            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApplicationId = new SelectList(db.Users, "Id", "Email", customer.ApplicationId);
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(/*[Bind(Include = "ID,ApplicationId,PickUpDay,FirstName,LastName,StreetAddress,City,State,ZipCode,Balance,ExtraPickUpDate,SuspendedStart,SuspendedEnd,PickUpConfirmation")]*/ Customer customer, int id)
        {
            if (ModelState.IsValid)
            {


                if (User.IsInRole("Customer"))
                {
                    var userId = User.Identity.GetUserId();
                    customer.ApplicationId = userId;
                    customer = await GetLatNLngAsync(customer);
                    db.Entry(customer).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Details","Customers");
                }
                if (User.IsInRole("Employee"))
                {
                    //var iDee = db.Customers.Find(id);
                    //customer.ApplicationId = iDee;
                    //db.Entry(customer).State = EntityState.Modified;
                    //db.SaveChanges();
                    Customer updatedCustomer = db.Customers.Find(id);
                    customer = await GetLatNLngAsync(customer);
                    updatedCustomer.PickupConfirmation = customer.PickupConfirmation;
                    if (updatedCustomer.PickupConfirmation == true)
                    {
                        updatedCustomer.Balance += (15 + customer.Balance);

                    }
                    db.SaveChanges();
                    return RedirectToAction("TodaysPickups","Employee");
                }


            }
            //ViewBag.ApplicationId = new SelectList(db.Users, "Id", "Email", customer.ApplicationId);
            return View(customer);
        }

        // GET: Customers/PICKUPINFO EDITING/5
        //public ActionResult PickupInfo(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Customer customer = db.Customers.Find(id);
        //    if (customer == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.ApplicationId = new SelectList(db.Users, "Id", "Email", customer.ApplicationId);
        //    return View(customer);
        //}

        //// POST: Customers/PICKUPINFO EDITING/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken] 
        //public ActionResult PickupInfo([Bind(Include = "PickupDay,ExtraPickupDate,SuspendedStart,SuspendedEnd,")] Customer customer)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // TODO: Add update logic here
        //        var userId = User.Identity.GetUserId();
        //        customer.ApplicationId = userId;
        //        db.Entry(customer).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Details");
        //    }
        //    return RedirectToAction("Index");

        //}

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
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


        public async System.Threading.Tasks.Task<Customer> GetLatNLngAsync(Customer customer)
        {
            var e = customer;
            string url = PrivateKeys.geoURLP1 + e.StreetAddress + ",+" + e.City + "+" + e.State + PrivateKeys.geoURLP2 + PrivateKeys.googleKey;
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            string jsonResult = await response.Content.ReadAsStringAsync();
            if (User.IsInRole("Customer"))
            {
                if (response.IsSuccessStatusCode)
                {
                    GeoCode location = JsonConvert.DeserializeObject<GeoCode>(jsonResult);
                    e.Lat = location.results[0].geometry.location.lat;
                    e.Lng = location.results[0].geometry.location.lng;
                    return e;
                }
            }
            if (User.IsInRole("Employee"))
            {
                return e;
            }

            db.Customers.Add(e);
            db.SaveChanges();
            return e;
        }


    }
}
