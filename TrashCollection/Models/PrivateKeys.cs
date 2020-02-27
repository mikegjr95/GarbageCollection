using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrashCollection.Models
{
    public class PrivateKeys
    {
        public static string googleKey = "AIzaSyBM7zAsr-3GVOlHhInOOOLVoYC2qSYio3o";

        public static string geoURLP1 = "https://maps.googleapis.com/maps/api/geocode/json?address=";

        public static string geoURLP2 = "&key=";

        public static string googleMap = "https://maps.googleapis.com/maps/api/js?" + geoURLP2 + googleKey + "&callback=initMap";

        //public static string directionsURLP1 = "https://maps.googleapis.com/maps/api/directions/json?";

        //public static string directionsURLP2 = "&origin=";

        //public static string youtubeURL1 = "https://www.googleapis.com/youtube/v3/search?part=snippet&q="; //youtube video name

        //public static string youtubeURL2 = "&type=video&key=";
    }
}