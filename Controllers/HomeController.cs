using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using finaldisaster.Models;

namespace finaldisaster.Controllers
{
    public class HomeController : Controller
    {
        static String connectionString = ConfigurationManager.ConnectionStrings
                                          ["ConMVC"].ConnectionString;
        SqlConnection con = new SqlConnection(connectionString);

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public String PopulateDonate(String name, String phnum, String material, int amt, int latitude, int longitude, String descrip)
        {

            String query = "INSERT INTO DONATE (fname,phonenum,material,amt,latitude,longitude,descrip) VALUES ('" + name + "','" + phnum + "','" + material + "'," + amt + "," + latitude + "," + longitude + ",'" + descrip + "')";

            try
            {
               con.Open();

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.ExecuteNonQuery();


                con.Close();

            }
            catch (Exception exc)
            {

                Console.Write("Exception occurred", exc);

            }

            return "Success";
        }

        public String PopulateNeed(String name, String phnum, String material, int amt, int pty, int latitude, int longitude, String descrip)
        {

            String query = "INSERT INTO NEED (fname,phonenum,material,amt,pty,latitude,longitude,descrip) VALUES ('" + name + "','" + phnum + "','" + material + "'," + amt + "," + pty + "," + latitude + "," + longitude + ",'" + descrip + "')";

            try
            {
                SqlConnection con = new SqlConnection(connectionString);

                con.Open();

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.ExecuteNonQuery();

                con.Close();

            }
            catch (Exception exc)
            {

                Console.Write("Exception occurred", exc);

            }

            return "Success";
        }

        public JsonResult matchDonate(String name, String phnum, String material, int amt, int pty, double latitude, double longitude, String descrip) {

            List<DonatePerson> matchpersonlist = new List<DonatePerson>();
            List<DonatePerson> finalmatchlist = new List<DonatePerson>();
            String query = "SELECT * FROM DONATE WHERE material='"+ material + "' AND amt >=" + amt ;

            SqlCommand cmd = new SqlCommand(query, con);
    
            con.Open();

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                DonatePerson newmatch = new DonatePerson();
                newmatch.name = dr["fname"].ToString();
                newmatch.phnum = dr["phonenum"].ToString();
                newmatch.material = dr["material"].ToString();
                newmatch.amt = Convert.ToInt32(dr["amt"]);
                newmatch.latitude = Convert.ToInt32(dr["latitude"]);
                newmatch.longitude = Convert.ToInt32(dr["longitude"]);
                newmatch.descrip = dr["descrip"].ToString();

                matchpersonlist.Add(newmatch);
            }
            
            con.Close();

            //Distance Calculation -- all the people within 5 miles
            foreach (DonatePerson dperson in matchpersonlist ) {

                double dist = getDistanceFromLatLonInKm(dperson.latitude, dperson.longitude,latitude,longitude);

                if ((dist*0.621371) <= 5.0) {    // if dist less than 5 miles, return that person

                    finalmatchlist.Add(dperson); 

                }

            }
            
            return Json(finalmatchlist,JsonRequestBehavior.AllowGet);
        }


        public NeedPerson matchNeed(String name, String phnum, String material, int amt, int latitude, int longitude, String descrip)
        {

            List<NeedPerson> matchpersonlist = new List<NeedPerson>();
            String query = "SELECT * FROM NEED WHERE material='" + material + "' AND amt <=" + amt;

            SqlCommand cmd = new SqlCommand(query, con);

            con.Open();

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                NeedPerson newmatch = new NeedPerson();
                newmatch.name = dr["fname"].ToString();
                newmatch.phnum = dr["phonenum"].ToString();
                newmatch.material = dr["material"].ToString();
                newmatch.amt = Convert.ToInt32(dr["amt"]);
                newmatch.latitude = Convert.ToInt32(dr["latitude"]);
                newmatch.longitude = Convert.ToInt32(dr["longitude"]);
                newmatch.descrip = dr["descrip"].ToString();

                matchpersonlist.Add(newmatch);
            }

            con.Close();

            
            //Distance Calculation -- all the people within 5 miles
            foreach (NeedPerson dperson in matchpersonlist)
            {

                double dist = getDistanceFromLatLonInKm(dperson.latitude, dperson.longitude, latitude, longitude);

                if ((dist * 0.621371) > 5.0)
                {    // if dist less than 5 miles, return that person

                    matchpersonlist.Remove(dperson);

                }

            }

            NeedPerson important = new NeedPerson();
            important = matchpersonlist[0];

            foreach (NeedPerson pers in matchpersonlist) {

                if ( pers.pty <= important.pty) {
                    important = pers;
                }
            }
            return important;
        }

        double getDistanceFromLatLonInKm(double lat1, double lon1, double lat2, double lon2)
        {
            float R = 6371; // Radius of the earth in km
            double dLat = deg2rad(lat2 - lat1);  // deg2rad below
            double dLon = deg2rad(lon2 - lon1);
            var a =
              Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
              Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) *
              Math.Sin(dLon / 2) * Math.Sin(dLon / 2)
              ;
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c; // Distance in km
            return d;
        }

        double deg2rad(double deg)
        {
            return deg * (Math.PI / 180);
        }

        public ActionResult Give()
        {
            return View();
        }

        public ActionResult Need()
        {
            return View();
        }
        public ActionResult FoodGive()
        {
            return View();
        }
        public ActionResult FoodNeed() {
            return View();
        }
        public ActionResult WaterGive() {
            return View();
        }
        public ActionResult WaterNeed() {
            return View();
        }
        public ActionResult GiveInfo() {
            return View();
        }
        public ActionResult NeedInfo() {
            return View();
        }
    }
}