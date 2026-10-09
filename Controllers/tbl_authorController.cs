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
    public class tbl_authorController : Controller
    {
        private dbBookEntities3 db = new dbBookEntities3();

        // GET: tbl_author
        public int GetAuthorID()
        {
            int ID = 0;
            if (Session["Email"] != null)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["dbconnection"].ToString();
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("Get_Author_ID", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@author_email", Session["Email"].ToString());
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    ID = Convert.ToInt32(reader["author_id"]);
                }
                connection.Close();
            }
            return ID;
        }

            // GET: tbl_author
            public ActionResult Index()
        {
            var tbl_author = db.tbl_author.Include(t => t.tbl_booktype);
            return View(tbl_author.ToList());
        }

        // GET: tbl_author/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Create", "Login");
            }
            id = GetAuthorID();
            tbl_author tbl_author = db.tbl_author.Find(id);
            if (tbl_author == null)
            {
                return HttpNotFound();
            }
            return View(tbl_author);
        }

        // GET: tbl_author/Create
        public ActionResult Create()
        {
            ViewBag.booktype_id = new SelectList(db.tbl_booktype, "booktype_id", "booktype_name");
            return View();
        }

        // POST: tbl_author/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "author_id,author_name,author_email,booktype_id,phone")] tbl_author tbl_author)
        {
            if (ModelState.IsValid)
            {
                db.tbl_author.Add(tbl_author);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.booktype_id = new SelectList(db.tbl_booktype, "booktype_id", "booktype_name", tbl_author.booktype_id);
            return View(tbl_author);
        }

        // GET: tbl_author/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_author tbl_author = db.tbl_author.Find(id);
            if (tbl_author == null)
            {
                return HttpNotFound();
            }
            ViewBag.booktype_id = new SelectList(db.tbl_booktype, "booktype_id", "booktype_name", tbl_author.booktype_id);
            return View(tbl_author);
        }

        // POST: tbl_author/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "author_id,author_name,author_email,booktype_id,phone")] tbl_author tbl_author)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_author).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.booktype_id = new SelectList(db.tbl_booktype, "booktype_id", "booktype_name", tbl_author.booktype_id);
            return View(tbl_author);
        }

        // GET: tbl_author/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_author tbl_author = db.tbl_author.Find(id);
            if (tbl_author == null)
            {
                return HttpNotFound();
            }
            return View(tbl_author);
        }

        // POST: tbl_author/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_author tbl_author = db.tbl_author.Find(id);
            db.tbl_author.Remove(tbl_author);
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
    }
}
