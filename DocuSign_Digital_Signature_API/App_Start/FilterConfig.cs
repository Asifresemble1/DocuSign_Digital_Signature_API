using System.Web;
using System.Web.Mvc;

namespace DocuSign_Digital_Signature_API
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
