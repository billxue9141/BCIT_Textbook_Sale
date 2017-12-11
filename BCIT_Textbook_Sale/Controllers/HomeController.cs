using BCIT_Textbook_Sale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using Microsoft.Owin.Security;

namespace BCIT_Textbook_Sale.Controllers
{
    public class HomeController : Controller
    {
        private TextbookDBEntities db = new TextbookDBEntities();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public HomeController()
        {
        }

        public HomeController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        public ActionResult Index()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            var programId = db.Programs.Include(id => id.programID);
            items.Add(new SelectListItem { Text = "Program", Value = "Program" });
            foreach(Program p in db.Programs)
            {
                items.Add(new SelectListItem { Text = p.programID, Value = p.programID });
            }
            ViewBag.proID = items;

            List<SelectListItem> buyorsell = new List<SelectListItem>();
            buyorsell.Add(new SelectListItem { Text = "Buy/Sell", Value = "Buy/Sell" });
            buyorsell.Add(new SelectListItem { Text = "Buy", Value = "Buy" });
            buyorsell.Add(new SelectListItem { Text = "Sell", Value = "Sell" });

            ViewBag.buyORsell = buyorsell;
            return View(db.Programs.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult BooksForSale(string proID, string buyORsell, string search)
        {
            List<SelectListItem> sortOptions = new List<SelectListItem>();
            sortOptions.Add(new SelectListItem { Text = "Descending Date", Value = "Descending Date" });
            sortOptions.Add(new SelectListItem { Text = "Ascending Date", Value = "Ascending Date" });
            sortOptions.Add(new SelectListItem { Text = "Price: Low to High", Value = "Price: Low to High" });
            sortOptions.Add(new SelectListItem { Text = "Price: High to Low", Value = "Price: High to Low" });
            ViewBag.sortOptions = sortOptions;
            if (proID == "Program" && buyORsell == "Buy/Sell" && search != "")
            {
                return View(db.Postings.Where(id => id.title.Contains(search)));
            }
            if(proID != "Program" && buyORsell == "Buy/Sell" && search == "")
            {
                return View(db.Postings.Where(id => id.programID.Contains(proID)));
            }
            if(proID == "Program" && buyORsell != "Buy/Sell" && search == "")
            {
                return View(db.Postings.Where(id => id.postingType.Contains(buyORsell)));
            }
            if(proID != "Program" && buyORsell != "Buy/Sell" && search == "")
            {
                return View(db.Postings.Where(id => id.programID.Contains(proID) && id.postingType.Contains(buyORsell)));
            }
            if(proID != "Program" && buyORsell == "Buy/Sell" && search != "")
            {
                return View(db.Postings.Where(id => id.programID.Contains(proID) && id.title.Contains(search)));
            }
            if(proID == "Program" && buyORsell != "Buy/Sell" && search != "")
            {
                return View(db.Postings.Where(id => id.postingType.Contains(buyORsell) && id.title.Contains(search)));
            }
            if (proID == "Program" && buyORsell == "Buy/Sell" && search == "")
            {
                var postings = db.Postings.Include(p => p.Program);
                return View(postings.ToList());
            }
                return View(db.Postings.Where(id=>id.programID.Contains(proID) && id.postingType.Contains(buyORsell) && id.title.Contains(search)));
        }

        public async Task<ActionResult> BooksOnRequest()
        {
            List<SelectListItem> sortOptions = new List<SelectListItem>();
            sortOptions.Add(new SelectListItem { Text = "Descending Date", Value = "Descending Date" });
            sortOptions.Add(new SelectListItem { Text = "Ascending Date", Value = "Ascending Date" });
            sortOptions.Add(new SelectListItem { Text = "Price: Low to High", Value = "Price: Low to High" });
            sortOptions.Add(new SelectListItem { Text = "Price: High to Low", Value = "Price: High to Low" });
            ViewBag.sortOptions = sortOptions;
            string UserEmail = await UserManager.GetEmailAsync(User.Identity.GetUserId());
            return View(db.Postings.Where(id => id.username.Contains(UserEmail)));
        }
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
        //
        // GET: /Manage/NewPost
        public ActionResult NewPost()
        {
            return View();
        }

        //TODO:
        //
        // POST: /Manage/NewPost
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NewPost(NewPostViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index");
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
            List<SelectListItem> buyorsell = new List<SelectListItem>();
            buyorsell.Add(new SelectListItem { Text = "Sell", Value = "Sell" });
            buyorsell.Add(new SelectListItem { Text = "Buy", Value = "Buy" });
            ViewBag.postingType = buyorsell;
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

    }
}