using System.Web;
using System.Web.Mvc;

namespace BCIT_Textbook_Sale
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
