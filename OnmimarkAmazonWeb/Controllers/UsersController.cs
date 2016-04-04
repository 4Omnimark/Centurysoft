using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using OmnimarkAmazon.Models;

namespace OmnimarkAmazonWeb.Controllers
{
    public class UsersController : _BaseController
    {
        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }

            base.Initialize(requestContext);
        }

        void PrepareRoles(Entities db, User u = null)
        {
            var Roles = db.aspnet_Roles.OrderBy(r => r.RoleName);

            if (u == null)
                ViewData["roles"] = Roles.ToList().Select(r => new SelectListItem { Text = r.RoleName, Value = Convert.ToString(r.RoleId), Selected = (Request.Form["cbRole" + r.RoleName] == "true,false") }).ToList();
            else
                ViewData["roles"] = Roles.ToList().Select(r => new SelectListItem { Text = r.RoleName, Value = Convert.ToString(r.RoleId), Selected = (u.aspnet_Users.aspnet_Roles.Contains(r)) }).ToList();

        }

        BaseRegisterModel LoadModel(Entities db, User u)
        {
            BaseRegisterModel brm = new BaseRegisterModel();
            Startbutton.Library.SetMatchingMembers(brm, u);
            brm.UserName = u.aspnet_Users.UserName;
            brm.Email = u.aspnet_Users.aspnet_Membership.Email;
            return brm;
        }

        //
        // GET: /Users/

        [Authorize(Roles="Administrator")]
        public ActionResult Index()
        {
            Entities db = new Entities();
            return View(db.Users.OrderBy(o => o.aspnet_Users.UserName));
        }

        //
        // GET: /Users/Create

        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            PrepareRoles(new Entities());
            ViewData["ButtonText"] = "Create";
            return View("Edit");
        } 

        //
        // POST: /Users/Create

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create(BaseRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus = MembershipService.CreateUser(model.UserName, model.Password, model.Email);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    UserProfile profile = UserProfile.GetUserProfile(model.UserName);
                    UpdateModel(profile.User);

                    List<aspnet_Roles> current_roles = profile.User.aspnet_Users.aspnet_Roles.ToList();

                    foreach (string k in Request.Form)
                        if (k.Length > 6)
                            if (k.Substring(0, 6) == "cbRole")
                                if (Request.Form[k] != "false")
                                    profile.User.aspnet_Users.aspnet_Roles.Add(profile.db.aspnet_Roles.Single(r => r.RoleName == k.Substring(6)));

                    profile.Save();

                    return RedirectToAction("Index", "Users");
                }
                else
                {
                    ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
                    PrepareRoles(new Entities());
                    ViewData["ButtonText"] = "Create";
                    return View("Edit");
                }
            }
            else
            {
                PrepareRoles(new Entities());
                ViewData["ButtonText"] = "Create";
                return View("Edit");
            }
        }


        //
        // GET: /Users/Details/5

        [Authorize(Roles = "Administrator")]
        public ActionResult Details(Guid id)
        {
            Entities db = new Entities();
            return View(db.Users.Single(a => a.UserID == id));
        }

        //
        // GET: /Users/Edit/5

        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(Guid id)
        {
            Entities db = new Entities();
            User u = db.Users.Single(a => a.UserID == id);
            PrepareRoles(db, u);

            ViewData["ButtonText"] = "Update";
            ViewData["PasswordNotRequired"] = true;
            return View(LoadModel(db, u));
        }

        //
        // POST: /Users/Edit/5

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(Guid id, BaseRegisterModel model)
        {
            Entities db = new Entities();

            if (ModelState.IsValid)
            {

                aspnet_Membership membershipRecord = db.aspnet_Membership.Single(m => m.UserId == id);
                membershipRecord.Email = model.Email;

                User editedUserRecord = db.Users.Single(a => a.UserID == id);
                Startbutton.Library.SetMatchingMembers(editedUserRecord, model);
                
                if (model.Password != null)
                    if (model.Password == model.ConfirmPassword)
                    {
                        MembershipUser mu = Membership.GetUser(membershipRecord.aspnet_Users.UserName);
                        string pwd = mu.ResetPassword();
                        mu.ChangePassword(pwd, model.Password);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Passwords do not match!");
                        PrepareRoles(db);
                        ViewData["PasswordNotRequired"] = true;
                        ViewData["ButtonText"] = "Update";
                        return View(model);
                    }
                
                List<aspnet_Roles> current_roles = editedUserRecord.aspnet_Users.aspnet_Roles.ToList();

                foreach (aspnet_Roles role in current_roles)
                    editedUserRecord.aspnet_Users.aspnet_Roles.Remove(role);

                foreach (string k in Request.Form)
                    if (k.Length > 6)
                        if (k.Substring(0, 6) == "cbRole")
                            if (Request.Form[k] != "false")
                                editedUserRecord.aspnet_Users.aspnet_Roles.Add(db.aspnet_Roles.Single(r => r.RoleName == k.Substring(6)));

                db.SaveChanges();

                if (editedUserRecord.aspnet_Users.UserName != model.UserName)
                    Startbutton.Web.Library.ChangeMembershipUserName(id, model.UserName);

                return RedirectToAction("Index");
            }
            else
            {
                PrepareRoles(db);
                ViewData["PasswordNotRequired"] = true;
                ViewData["ButtonText"] = "Update";
                return View(model);
            }
        }

        //
        // GET: /Users/Delete/5

        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(Guid id)
        {
            Entities db = new Entities();
            return View(db.Users.Single(a => a.UserID == id));
        }

        //
        // POST: /Users/Delete/5

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(Guid id, FormCollection collection)
        {
            Entities db = new Entities();

            User user = db.Users.Single(a => a.UserID == id);

            List<aspnet_Roles> current_roles = user.aspnet_Users.aspnet_Roles.ToList();

            foreach (aspnet_Roles role in current_roles)
                user.aspnet_Users.aspnet_Roles.Remove(role);

            db.Users.Remove(user);
            db.aspnet_Membership.Remove(db.aspnet_Membership.Single(m => m.UserId == id));
            db.aspnet_Users.Remove(db.aspnet_Users.Single(u => u.UserId == id));
            db.SaveChanges();
 
            return RedirectToAction("Index");
        }
    }
}
