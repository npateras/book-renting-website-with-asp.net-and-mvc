﻿using MVCBookshelf.Models;
using PagedList;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MVCBookshelf.Controllers
{
    public class TitleauthorsController : Controller
    {
        private pubsEntities db = new pubsEntities();

        // GET: Titleauthors
        public ActionResult Index(string search, int? i)
        {
            var titleauthor = db.titleauthor.Include(t => t.authors).Include(t => t.titles);
            return View(db.titleauthor.Where(x => x.royaltyper.ToString().StartsWith(search)
                                            || x.au_ord.ToString().StartsWith(search)
                                            || x.authors.au_lname.StartsWith(search)
                                            || x.titles.title.StartsWith(search)
                                            || search == null).ToList().ToPagedList(i ?? 1, 5));
        }

        // GET: Titleauthors/Create
        public ActionResult Create()
        {
            ViewBag.au_id = new SelectList(db.authors, "au_id", "au_lname");
            ViewBag.title_id = new SelectList(db.titles, "title_id", "title");
            return View();
        }

        // POST: Titleauthors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "au_id,title_id,au_ord,royaltyper")] titleauthor titleauthor)
        {
            if (ModelState.IsValid)
            {
                db.titleauthor.Add(titleauthor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.au_id = new SelectList(db.authors, "au_id", "au_lname", titleauthor.au_id);
            ViewBag.title_id = new SelectList(db.titles, "title_id", "title", titleauthor.title_id);
            return View(titleauthor);
        }

        // GET: Titleauthors/Edit/5
        public ActionResult Edit(string au_id, string title_id)
        {
            if (au_id == null || title_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            titleauthor titleauthor = db.titleauthor.Find(au_id, title_id);
            if (titleauthor == null)
            {
                return HttpNotFound();
            }
            ViewBag.au_id = new SelectList(db.authors, "au_id", "au_lname", titleauthor.au_id);
            ViewBag.title_id = new SelectList(db.titles, "title_id", "title", titleauthor.title_id);
            return View(titleauthor);
        }

        // POST: Titleauthors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "au_id,title_id,au_ord,royaltyper")] titleauthor titleauthor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(titleauthor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.au_id = new SelectList(db.authors, "au_id", "au_lname", titleauthor.au_id);
            ViewBag.title_id = new SelectList(db.titles, "title_id", "title", titleauthor.title_id);
            return View(titleauthor);
        }

        // GET: Titleauthors/Delete/5
        public ActionResult Delete(string au_id, string title_id)
        {
            if (au_id == null || title_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            titleauthor titleauthor = db.titleauthor.Find(au_id, title_id);
            if (titleauthor == null)
            {
                return HttpNotFound();
            }
            return View(titleauthor);
        }

        // POST: Titleauthors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string au_id, string title_id)
        {
            titleauthor titleauthor = db.titleauthor.Find(au_id, title_id);
            db.titleauthor.Remove(titleauthor);
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