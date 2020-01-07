using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrashCollection.Models
{
    [key]
    public class Customer
    {
        public DateTime PickupDay { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ExtraPickupDate { get; set; }
        public double StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int ZipCode { get; set; }
        public double Balance { get; set; }
        public DateTime SuspendedStart { get; set; }
        public DateTime SuspendedEnd { get; set; }
        public bool PickupConfirmation { get; set; }

    }
}