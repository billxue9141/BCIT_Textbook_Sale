using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BCIT_Textbook_Sale.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.IO;

namespace BCIT_Textbook_Sale.Controllers
{
    public class PostingsController : Controller
    {
        private TextbookDBEntities db = new TextbookDBEntities();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public PostingsController()
        {
        }

        public PostingsController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Postings
        public ActionResult Index()
        {
            var postings = db.Postings.Include(p => p.Program).OrderByDescending(p => p.postdate);
            List<SelectListItem> sortOptions = new List<SelectListItem>();
            sortOptions.Add(new SelectListItem { Text = "Descending Date", Value = "Descending Date" });
            sortOptions.Add(new SelectListItem { Text = "Ascending Date", Value = "Ascending Date" });
            sortOptions.Add(new SelectListItem { Text = "Price: Low to High", Value = "Price: Low to High" });
            sortOptions.Add(new SelectListItem { Text = "Price: High to Low", Value = "Price: High to Low" });
            ViewBag.sortOptions = sortOptions;
            return View(postings.ToList());
        }

        // GET: Postings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Posting posting = db.Postings.Find(id);
            if (posting == null)
            {
                return HttpNotFound();
            }
            return View(posting);
        }

        // GET: Postings/Create
        public ActionResult Create()
        {
            ViewBag.programID = new SelectList(db.Programs, "programID", "programName");
            List<SelectListItem> buyorsell = new List<SelectListItem>();
            buyorsell.Add(new SelectListItem { Text = "Sell", Value = "Sell" });
            buyorsell.Add(new SelectListItem { Text = "Buy", Value = "Buy" });
            ViewBag.postingType = buyorsell;
            return View();
        }

        // POST: Postings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,title,username,postdate,description,programID, postingType, imglink, price")] Posting posting)
        {
            if (ModelState.IsValid)
            {
                posting.postdate = DateTime.Now;
                posting.username = await UserManager.GetEmailAsync(User.Identity.GetUserId());
                if (db.Postings.OrderByDescending(u => u.Id).FirstOrDefault() != null)
                    posting.Id = db.Postings.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
                else
                    posting.Id = 0;

                db.Postings.Add(posting);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.programID = new SelectList(db.Programs, "programID", "programName", posting.programID);
            return View(posting);
        }

        // GET: Postings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Posting posting = db.Postings.Find(id);
            if (posting == null)
            {
                return HttpNotFound();
            }
            ViewBag.programID = new SelectList(db.Programs, "programID", "programName", posting.programID);
            return View(posting);
        }

        // POST: Postings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,title,username,postdate,description,programID")] Posting posting)
        {
            if (ModelState.IsValid)
            {
                db.Entry(posting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.programID = new SelectList(db.Programs, "programID", "programName", posting.programID);
            return View(posting);
        }

        // GET: Postings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Posting posting = db.Postings.Find(id);
            if (posting == null)
            {
                return HttpNotFound();
            }
            return View(posting);
        }

        // POST: Postings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Posting posting = db.Postings.Find(id);
            db.Postings.Remove(posting);
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
