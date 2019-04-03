using System.Web;
using System.Web.Mvc;
using UserSubscriptionsManagement.WebAPI.Filters;

namespace UserSubscriptionsManagement.WebAPI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
