﻿using MVCBookshelf.Models;
using PagedList;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MVCBookshelf.Controllers
{
    public class PublishersController : Controller
    {
        private pubsEntities db = new pubsEntities();

        // GET: Publishers
        public ActionResult Index(string search, int? i)
        {
            var publishers = db.publishers.Include(p => p.pub_info);

            return View(db.publishers.Where(x => x.pub_name.StartsWith(search)
                                            || x.city.StartsWith(search)
                                            || x.state.StartsWith(search)
                                            || x.country.StartsWith(search)
                                            || x.pub_info.pr_info.StartsWith(search)
                                            || search == null).ToList().ToPagedList(i ?? 1, 5));
        }

        // GET: Publishers/Details
        public ActionResult Details(string pub_id)
        {
            if (pub_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pub_info publisherInfo = db.pub_info.Find(pub_id);
            if (publisherInfo == null)
            {
                return HttpNotFound();
            }
            ViewBag.pub_id = new SelectList(db.pub_info, "pub_id", "pr_info");
            return View(publisherInfo);
        }

        // GET: Publishers/Create
        public ActionResult Create()
        {
            ViewBag.pub_id = new SelectList(db.pub_info, "pub_id", "pr_info");
            return View();
        }

        // POST: Publishers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "pub_id,pub_name,city,state,country")] publishers publishers)
        {
            if (ModelState.IsValid)
            {
                db.publishers.Add(publishers);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.pub_id = new SelectList(db.pub_info, "pub_id", "pr_info", publishers.pub_id);
            return View(publishers);
        }

        // GET: Publishers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            publishers publishers = db.publishers.Find(id);
            if (publishers == null)
            {
                return HttpNotFound();
            }
            ViewBag.pub_id = new SelectList(db.pub_info, "pub_id", "pr_info", publishers.pub_id);
            return View(publishers);
        }

        // POST: Publishers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "pub_id,pub_name,city,state,country")] publishers publishers, [Bind(Include = "logo,pr_info")] pub_info pubInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(publishers).State = EntityState.Modified;
                db.Entry(pubInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.pub_id = new SelectList(db.pub_info, "pub_id", "pr_info", publishers.pub_id);
            return View(publishers);
        }

        // GET: Publishers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            publishers publishers = db.publishers.Find(id);
            if (publishers == null)
            {
                return HttpNotFound();
            }
            return View(publishers);
        }

        // POST: Publishers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            publishers publishers = db.publishers.Find(id);
            db.publishers.Remove(publishers);
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