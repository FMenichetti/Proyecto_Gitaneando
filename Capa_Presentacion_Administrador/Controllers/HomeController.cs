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

        [HttpPost]
        public JsonResult GuardarUsuario(Usuario user)
        {
            object resultado;
            string mensaje = string.Empty;

            if (user.IdUsuario == 0)
            {

                resultado = new NegocioUsuarios().RegistrarUsuarioConSP(user, out mensaje);
            }
            else
            {
                resultado = new NegocioUsuarios().EditarUsuarioConSp(user, out mensaje);

            }

            return Json(new { resultado = resultado , mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
    }
}