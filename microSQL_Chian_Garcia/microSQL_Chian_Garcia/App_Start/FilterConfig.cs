using System.Web;
using System.Web.Mvc;

namespace microSQL_Chian_Garcia
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
