using System.Web;
using System.Web.Mvc;

namespace Capa_Presentacion_Administrador
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
