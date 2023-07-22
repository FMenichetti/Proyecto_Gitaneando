using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaNegocio;
using CapaDominio;

namespace Capa_Presentacion_Administrador.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Usuarios()
        {
            return View();
        }
        [HttpGet]
        public JsonResult ListarUsuarios()
        {
            List<Usuario> listaUsuarios = new List<Usuario>();
            listaUsuarios = new NegocioUsuarios().Listar();
            //Lo de arriba es lo mismo  pero esta abreviado
            //NegocioUsuarios negocioUsuarios = new NegocioUsuarios();
            //listaUsuarios = negocioUsuarios.Listar();
            return Json( new { data = listaUsuarios } , JsonRequestBehavior.AllowGet);

            

        }
    }
}