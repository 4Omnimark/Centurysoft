using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Profile;
using System.Web.Security;
using System.Collections;
using OmnimarkAmazon.Models;
using System.Reflection;

namespace OmnimarkAmazon.Models
{
    public class UserProfile : ProfileBase
    {

        public User User;
        public Entities db = new Entities();

        public override void Save()
        {
            base.Save();
            db.SaveChanges();
        }

        //public new static ProfileBase Create(string username)
        //{
        //    // call base Create by reflection due to lacking of .NET environment of calling base static methods
        //    Type profilebase = typeof(UserProfile).BaseType;
        //    MethodInfo[] methods = profilebase.GetMethods();
        //    MethodInfo create = methods.First(m => m.Name == "Create" && m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType == typeof(string));
        //    UserProfile rtn = (UserProfile)create.Invoke(null, new object[] { username });

        //    aspnet_Users anu = rtn.db.aspnet_Users.Single(u => u.UserName == username);

        //    rtn.User = anu.User;

        //    //if (rtn.User == null)
        //    //{
        //    //    rtn.User = User.CreateUser(anu.UserId, DateTime.Now);
        //    //    rtn.db.Users.Add(rtn.User);
        //    //    rtn.db.SaveChanges();
        //    //}

        //    return rtn;
        //}

        public static UserProfile GetUserProfile(string username)
        {
            return Create(username) as UserProfile;
        }

        public static UserProfile GetUserProfile()
        {
            if (Membership.GetUser() != null)
                return Create(Membership.GetUser().UserName) as UserProfile;
            else
                return null;
        }

        public static UserProfile Current
        {
            get { return GetUserProfile(); }
        }

    }
}