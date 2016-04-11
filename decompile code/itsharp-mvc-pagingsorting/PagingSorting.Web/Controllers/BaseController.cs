using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagingSorting.Web.Models;
using System.Text;

namespace PagingSorting.Web.Controllers
{
    public class BaseController : Controller
    {
        //
        // GET: /Base/
        public IQueryable<People> PeopleList { get; set; }
        public const int PageSize = 30;


        public BaseController()
        {
            PeopleList = GetTestData();

        }

        //private IQueryable<People> InitPeopleList()
        //{
        //    List<People> list = new List<People>();
        //    string[] names = new string[]{ "Tom", "Kevin", "Luke", "Neil", "Mic","Rob" };
        //    Random random = new Random();
        //    for (int i = 0; i < 500; i++)
        //    {
        //        list.Add(new People { Username = string.Format("{0}{1}", names[random.Next(0, 5)],random.Next(0,100)), Id = i, Age = random.Next(0, 100) });
        //    }
        //    return list.AsQueryable();
        //}

        #region create test data
        private IQueryable<People> GetTestData()
        {
            List<People> list = new List<People>();
            list.Add(new People { Username = "Kevin22", Id = 0, Age = 99 });
            list.Add(new People { Username = "Luke98", Id = 1, Age = 60 });
            list.Add(new People { Username = "Neil39", Id = 2, Age = 60 });
            list.Add(new People { Username = "Tom41", Id = 3, Age = 76 });
            list.Add(new People { Username = "Neil71", Id = 4, Age = 36 });
            list.Add(new People { Username = "Tom38", Id = 5, Age = 44 });
            list.Add(new People { Username = "Tom42", Id = 6, Age = 58 });
            list.Add(new People { Username = "Kevin68", Id = 7, Age = 90 });
            list.Add(new People { Username = "Luke21", Id = 8, Age = 33 });
            list.Add(new People { Username = "Luke12", Id = 9, Age = 28 });
            list.Add(new People { Username = "Luke10", Id = 10, Age = 99 });
            list.Add(new People { Username = "Neil14", Id = 11, Age = 79 });
            list.Add(new People { Username = "Neil12", Id = 12, Age = 82 });
            list.Add(new People { Username = "Kevin1", Id = 13, Age = 91 });
            list.Add(new People { Username = "Kevin46", Id = 14, Age = 45 });
            list.Add(new People { Username = "Kevin91", Id = 15, Age = 35 });
            list.Add(new People { Username = "Tom88", Id = 16, Age = 80 });
            list.Add(new People { Username = "Mic20", Id = 17, Age = 26 });
            list.Add(new People { Username = "Tom1", Id = 18, Age = 54 });
            list.Add(new People { Username = "Tom97", Id = 19, Age = 77 });
            list.Add(new People { Username = "Kevin34", Id = 20, Age = 26 });
            list.Add(new People { Username = "Kevin66", Id = 21, Age = 30 });
            list.Add(new People { Username = "Neil9", Id = 22, Age = 57 });
            list.Add(new People { Username = "Luke38", Id = 23, Age = 26 });
            list.Add(new People { Username = "Neil79", Id = 24, Age = 40 });
            list.Add(new People { Username = "Neil4", Id = 25, Age = 21 });
            list.Add(new People { Username = "Luke13", Id = 26, Age = 30 });
            list.Add(new People { Username = "Mic24", Id = 27, Age = 24 });
            list.Add(new People { Username = "Luke50", Id = 28, Age = 90 });
            list.Add(new People { Username = "Neil47", Id = 29, Age = 13 });
            list.Add(new People { Username = "Kevin68", Id = 30, Age = 14 });
            list.Add(new People { Username = "Tom3", Id = 31, Age = 67 });
            list.Add(new People { Username = "Neil88", Id = 32, Age = 80 });
            list.Add(new People { Username = "Tom62", Id = 33, Age = 81 });
            list.Add(new People { Username = "Neil58", Id = 34, Age = 50 });
            list.Add(new People { Username = "Luke29", Id = 35, Age = 41 });
            list.Add(new People { Username = "Mic47", Id = 36, Age = 96 });
            list.Add(new People { Username = "Kevin62", Id = 37, Age = 83 });
            list.Add(new People { Username = "Luke28", Id = 38, Age = 10 });
            list.Add(new People { Username = "Tom84", Id = 39, Age = 15 });
            list.Add(new People { Username = "Luke3", Id = 40, Age = 62 });
            list.Add(new People { Username = "Luke32", Id = 41, Age = 69 });
            list.Add(new People { Username = "Tom58", Id = 42, Age = 76 });
            list.Add(new People { Username = "Neil2", Id = 43, Age = 15 });
            list.Add(new People { Username = "Luke31", Id = 44, Age = 51 });
            list.Add(new People { Username = "Luke19", Id = 45, Age = 65 });
            list.Add(new People { Username = "Neil94", Id = 46, Age = 21 });
            list.Add(new People { Username = "Luke86", Id = 47, Age = 0 });
            list.Add(new People { Username = "Tom91", Id = 48, Age = 6 });
            list.Add(new People { Username = "Kevin57", Id = 49, Age = 74 });
            list.Add(new People { Username = "Luke61", Id = 50, Age = 4 });
            list.Add(new People { Username = "Neil73", Id = 51, Age = 58 });
            list.Add(new People { Username = "Tom35", Id = 52, Age = 26 });
            list.Add(new People { Username = "Mic42", Id = 53, Age = 71 });
            list.Add(new People { Username = "Neil13", Id = 54, Age = 45 });
            list.Add(new People { Username = "Mic91", Id = 55, Age = 31 });
            list.Add(new People { Username = "Kevin99", Id = 56, Age = 9 });
            list.Add(new People { Username = "Luke29", Id = 57, Age = 89 });
            list.Add(new People { Username = "Mic92", Id = 58, Age = 17 });
            list.Add(new People { Username = "Neil25", Id = 59, Age = 40 });
            list.Add(new People { Username = "Neil80", Id = 60, Age = 0 });
            list.Add(new People { Username = "Tom15", Id = 61, Age = 40 });
            list.Add(new People { Username = "Tom75", Id = 62, Age = 58 });
            list.Add(new People { Username = "Mic28", Id = 63, Age = 83 });
            list.Add(new People { Username = "Kevin92", Id = 64, Age = 52 });
            list.Add(new People { Username = "Luke83", Id = 65, Age = 72 });
            list.Add(new People { Username = "Luke35", Id = 66, Age = 99 });
            list.Add(new People { Username = "Neil98", Id = 67, Age = 58 });
            list.Add(new People { Username = "Neil12", Id = 68, Age = 32 });
            list.Add(new People { Username = "Tom71", Id = 69, Age = 81 });
            list.Add(new People { Username = "Luke57", Id = 70, Age = 9 });
            list.Add(new People { Username = "Mic17", Id = 71, Age = 61 });
            list.Add(new People { Username = "Neil63", Id = 72, Age = 97 });
            list.Add(new People { Username = "Tom70", Id = 73, Age = 16 });
            list.Add(new People { Username = "Neil38", Id = 74, Age = 70 });
            list.Add(new People { Username = "Kevin5", Id = 75, Age = 36 });
            list.Add(new People { Username = "Kevin43", Id = 76, Age = 8 });
            list.Add(new People { Username = "Luke6", Id = 77, Age = 90 });
            list.Add(new People { Username = "Luke88", Id = 78, Age = 82 });
            list.Add(new People { Username = "Luke35", Id = 79, Age = 3 });
            list.Add(new People { Username = "Tom95", Id = 80, Age = 4 });
            list.Add(new People { Username = "Neil52", Id = 81, Age = 70 });
            list.Add(new People { Username = "Neil54", Id = 82, Age = 75 });
            list.Add(new People { Username = "Mic79", Id = 83, Age = 19 });
            list.Add(new People { Username = "Neil50", Id = 84, Age = 65 });
            list.Add(new People { Username = "Mic1", Id = 85, Age = 59 });
            list.Add(new People { Username = "Mic40", Id = 86, Age = 7 });
            list.Add(new People { Username = "Mic78", Id = 87, Age = 27 });
            list.Add(new People { Username = "Neil95", Id = 88, Age = 51 });
            list.Add(new People { Username = "Tom44", Id = 89, Age = 29 });
            list.Add(new People { Username = "Neil27", Id = 90, Age = 28 });
            list.Add(new People { Username = "Mic97", Id = 91, Age = 75 });
            list.Add(new People { Username = "Tom96", Id = 92, Age = 86 });
            list.Add(new People { Username = "Mic51", Id = 93, Age = 50 });
            list.Add(new People { Username = "Neil45", Id = 94, Age = 64 });
            list.Add(new People { Username = "Mic69", Id = 95, Age = 55 });
            list.Add(new People { Username = "Kevin57", Id = 96, Age = 86 });
            list.Add(new People { Username = "Kevin54", Id = 97, Age = 95 });
            list.Add(new People { Username = "Mic11", Id = 98, Age = 16 });
            list.Add(new People { Username = "Neil3", Id = 99, Age = 56 });
            return list.AsQueryable();
        }
        #endregion

    }
}
