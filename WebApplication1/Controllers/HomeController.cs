using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        readonly ConnectDB db;

        public HomeController()
        {
            this.db = new ConnectDB();
        }

        public ActionResult Index()
        {
            if(Session["CustomerID"]!= null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult About()
        {
            return View();

        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        //GET: Register

        public ActionResult Register()
        {
            return View();
        }

        //POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Customer _user)
        {
            if (ModelState.IsValid)
            {
                var check = db.Customers.FirstOrDefault(s => s.Email == _user.Email);
                if (check == null)
                {
                    _user.Password = GetMD5(_user.Password);
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.Customers.Add(_user);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Email already exists";
                    return View();
                }


            }
            return View();


        }

        //create a string MD5
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }
        public ActionResult Login()
        {
            return View();
        }



       
        //Logout
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login", "Home");
        }



        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            if (ModelState.IsValid)
            {
                var f_password = GetMD5(password);

                // Check if the user is a customer
                var customerData = db.Customers.Where(s => s.Email.Equals(email) && s.Password.Equals(f_password)).ToList();
                if (customerData.Count() > 0)
                {
                    // Add session for customer
                    var customer = customerData.FirstOrDefault();
                    Session["FullName"] = customer.FirstName + " " + customer.LastName;
                    Session["Email"] = customer.Email;
                    Session["CustomerID"] = customer.CustomerID;
                    return RedirectToAction("Index");
                }

                // Check if the user is an admin
                var adminData = db.Admins.Where(s => s.Email.Equals(email) && s.Password.Equals(f_password)).ToList();
                if (adminData.Count() > 0)
                {
                    // Add session for admin
                    var admin = adminData.FirstOrDefault();
                    Session["AdminFullName"] = admin.FirstName + " " + admin.LastName;
                    Session["AdminEmail"] = admin.Email;
                    Session["AdminID"] = admin.AdminID;
                    return RedirectToAction("AdminDashboard");
                }

                // If no match found
                ViewBag.error = "Login failed";
                return View();
            }
            return View();
        }

        public ActionResult AdminDashboard()
        {
            if (Session["AdminID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
    }

}
