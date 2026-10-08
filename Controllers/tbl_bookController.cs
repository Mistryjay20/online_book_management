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
        public ActionResult Create(int? booktype_id,string book_name)
        {
            var book = new tbl_book
            {
                book_name = book_name
            };
            ViewBag.booktype_id = new SelectList(db.tbl_booktype, "booktype_id", "booktype_name", booktype_id);
            List<SelectListItem> authors = db.tbl_author
                .Where(a => a.booktype_id == booktype_id)
                .Select(a => new SelectListItem
                {
                    Text = a.author_name,
                    Value = a.author_id.ToString()
                }).ToList();
            ViewBag.author_id = authors;
            ViewBag.pub_id = new SelectList(db.tbl_publisher, "pub_id", "pub_name");
            return View(book);
        }

        // POST: tbl_book/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "book_id,book_name,booktype_id,author_id,pub_id,book_pub_date,ISBN_No,Price")] tbl_book tbl_book)
        {
            try
            {


                if (ModelState.IsValid)
                {
                    var type = db.tbl_booktype.Find(tbl_book.booktype_id);
                    if (type.booktype_name == "Fiction")
                    {
                        tbl_book.Price = tbl_book.Price - (tbl_book.Price * 10 / 100);
                    }
                    db.tbl_book.Add(tbl_book);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.author_id = new SelectList(db.tbl_author, "author_id", "author_name", tbl_book.author_id);
                ViewBag.booktype_id = new SelectList(db.tbl_booktype, "booktype_id", "booktype_name", tbl_book.booktype_id);
                ViewBag.pub_id = new SelectList(db.tbl_publisher, "pub_id", "pub_name", tbl_book.pub_id);
                return View(tbl_book);
            }
            catch (Exception ex)
            {
                ViewBag.error = "Error:" + ex;
                return View();
            }
            }
            

        // GET: tbl_book/Edit/5
        public ActionResult Edit(int? id, int? booktype_id)
        {
            tbl_book tbl_book = db.tbl_book.Find(id);

            if (booktype_id == null)
            {
                booktype_id = tbl_book.booktype_id;
            }

            ViewBag.booktype_id = new SelectList(db.tbl_booktype, "booktype_id", "booktype_name", booktype_id);

            List<SelectListItem> authors = db.tbl_author
                .Where(a => a.booktype_id == booktype_id)
                .Select(a => new SelectListItem
                {
                    Text = a.author_name,
                    Value = a.author_id.ToString(),
                    Selected = a.author_id == tbl_book.author_id
                }).ToList();

            ViewBag.author_id = authors;

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
