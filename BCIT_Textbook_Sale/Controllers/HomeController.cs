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
using BCIT_Textbook_Sale.Models;

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

        public ActionResult BooksForSale()
        {
            ViewBag.Message = "Here are all the books for sale at the moment.";

            return View();
        }

        public ActionResult BooksOnRequest()
        {
            ViewBag.Message = "People are looking for these books.";

            return View();
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


    }
}