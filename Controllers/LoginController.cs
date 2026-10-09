using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using online_book_management.Models;

namespace online_book_management.Controllers
{
    public class LoginController : Controller
    {
        private dbBookEntities3 db = new dbBookEntities3();

        // GET: Login
        public ActionResult Index()
        {
            if (Session["Email"] != null)
            {
                ViewBag.Welcome = "Welcome, " + Session["Email"];
            }
            return View();
        }

        // GET: Login/Details/5
        public ActionResult Details()
        {
            return View();
        }

        // GET: Login/Create
        public ActionResult Create()
        { 
            return View();
        }

        // POST: Login/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                tbl_author auth = new tbl_author();
                auth.author_email = collection["author_email"];
                auth.password = collection["password"];
                if (auth.author_email == null)
                {
                    ViewBag.error = "Please enter the email";
                }
                else if (auth.author_email == null)
                {
                    ViewBag.error = "Please enter the password";
                }
                else
                {
                    string connectionString = ConfigurationManager.ConnectionStrings["dbconnection"].ToString();
                    SqlConnection connection = new SqlConnection(connectionString);
                    SqlCommand cmd = new SqlCommand("Auth_log", connection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    connection.Open();
                    cmd.Parameters.AddWithValue("@author_email", auth.author_email);
                    cmd.Parameters.AddWithValue("@password", auth.password);
                    int i = Convert.ToInt32(cmd.ExecuteScalar());
                    if (i > 0)
                    {
                        Session["Email"] = auth.author_email;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.error = "Invalid Email or Password";
                        return View(auth);
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                {
                    ViewBag.error = ex + "Error";
                    return View();

                }
            }
        }

        // GET: Login/Edit/5
        public ActionResult Edit()
        {
            return View();
        }

        // POST: Login/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Login/Delete/5
        public ActionResult Delete()
        {
           
            return View();
        }

        // POST: Login/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            };
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
