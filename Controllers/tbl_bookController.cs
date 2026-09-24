using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using online_book_management.Models;

namespace online_book_management.Controllers
{
    public class tbl_bookController : Controller
    {
        private dbBookEntities db = new dbBookEntities();

        // GET: tbl_book
        public ActionResult Index()
        {
            var tbl_book = db.tbl_book.Include(t => t.tbl_author).Include(t => t.tbl_booktype).Include(t => t.tbl_publisher);
            return View(tbl_book.ToList());
        }

        // GET: tbl_book/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_book tbl_book = db.tbl_book.Find(id);
            if (tbl_book == null)
            {
                return HttpNotFound();
            }
            return View(tbl_book);
        }

        // GET: tbl_book/Create
        public ActionResult Create()
        {
            ViewBag.author_id = new SelectList(db.tbl_author, "author_id", "author_name");
            ViewBag.booktype_id = new SelectList(db.tbl_booktype, "booktype_id", "booktype_name");
            ViewBag.pub_id = new SelectList(db.tbl_publisher, "pub_id", "pub_name");
            return View();
        }

        // POST: tbl_book/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "book_id,book_name,booktype_id,author_id,pub_id,book_pub_date,ISBN_No,Price")] tbl_book tbl_book)
        {
            if (ModelState.IsValid)
            {
                db.tbl_book.Add(tbl_book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.author_id = new SelectList(db.tbl_author, "author_id", "author_name", tbl_book.author_id);
            ViewBag.booktype_id = new SelectList(db.tbl_booktype, "booktype_id", "booktype_name", tbl_book.booktype_id);
            ViewBag.pub_id = new SelectList(db.tbl_publisher, "pub_id", "pub_name", tbl_book.pub_id);
            return View(tbl_book);
        }

        // GET: tbl_book/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_book tbl_book = db.tbl_book.Find(id);
            if (tbl_book == null)
            {
                return HttpNotFound();
            }
            ViewBag.author_id = new SelectList(db.tbl_author, "author_id", "author_name", tbl_book.author_id);
            ViewBag.booktype_id = new SelectList(db.tbl_booktype, "booktype_id", "booktype_name", tbl_book.booktype_id);
            ViewBag.pub_id = new SelectList(db.tbl_publisher, "pub_id", "pub_name", tbl_book.pub_id);
            return View(tbl_book);
        }

        // POST: tbl_book/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "book_id,book_name,booktype_id,author_id,pub_id,book_pub_date,ISBN_No,Price")] tbl_book tbl_book)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.author_id = new SelectList(db.tbl_author, "author_id", "author_name", tbl_book.author_id);
            ViewBag.booktype_id = new SelectList(db.tbl_booktype, "booktype_id", "booktype_name", tbl_book.booktype_id);
            ViewBag.pub_id = new SelectList(db.tbl_publisher, "pub_id", "pub_name", tbl_book.pub_id);
            return View(tbl_book);
        }

        // GET: tbl_book/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_book tbl_book = db.tbl_book.Find(id);
            if (tbl_book == null)
            {
                return HttpNotFound();
            }
            return View(tbl_book);
        }

        // POST: tbl_book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_book tbl_book = db.tbl_book.Find(id);
            db.tbl_book.Remove(tbl_book);
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
