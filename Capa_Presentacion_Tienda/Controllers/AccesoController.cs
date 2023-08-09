using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CapaDominio;
using CapaNegocio;


namespace Capa_Presentacion_Tienda.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Registrar()
        {
            return View();
        }


        public ActionResult Restablecer()
        {
            return View();
        }
        public ActionResult CambiarClave()
        {
            return View();

        }
        [HttpPost]
        public ActionResult Registrar(Cliente cliente)
        {
            int resultado;
            string mensaje = string.Empty;

            ViewData["Nombre"] = string.IsNullOrEmpty(cliente.Nombre);
            ViewData["Apellido"] = string.IsNullOrEmpty(cliente.Apellido);
            ViewData["Email"] = string.IsNullOrEmpty(cliente.Email);

            if (cliente.Pass != cliente.ConfirmarPass)
            {
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }

            resultado = new NegocioCliente().RegistrarClienteConSP(cliente, out mensaje);

            if (resultado > 0)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }
        }
        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
            Cliente cliente;
            cliente = new NegocioCliente().Listar().Where(x => x.Email == correo && x.Pass == NegocioRecursos.ConvertirSHA256(clave)).FirstOrDefault();

            if (cliente == null)
            {
                ViewBag.Error = "Correo o contraseña incorrectos";
            return View();

            }
            else
            {
                if (cliente.Restablecer)
                {
                    TempData["idCliente"] = cliente.IdCliente;
                    return RedirectToAction("CambiarClave", "Acceso");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(cliente.Email, false);
                    Session["cliente"] = cliente;
                    ViewBag.Error = null;
                    return RedirectToAction("Index", "Tienda");
                }
            }

        }

        [HttpPost]
        public ActionResult Restablecer(string correo)
        {
            Cliente cliente = new Cliente();
            cliente = new NegocioCliente().Listar().Where(x => x.Email == correo).FirstOrDefault();

            if (cliente == null)
            {
                ViewBag.Error = "No se encontro el Cliente relacionado a ese Correo";
                return View();
            }

            string mensaje = string.Empty;
            bool respuesta = new NegocioCliente().RestablecerClave(cliente.IdCliente, correo, out mensaje);

            if (respuesta)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();

            }
        }
        [HttpPost]
        public ActionResult CambiarClave(string idCliente, string claveActual, string claveNueva, string confirmarClave)
        {
            Cliente cliente = new Cliente();
            cliente = new NegocioCliente().Listar().Where(i => i.IdCliente == int.Parse(idCliente)).FirstOrDefault();
            if (cliente.Pass != NegocioRecursos.ConvertirSHA256(claveActual))
            {
                TempData["idCliente"] = idCliente;
                ViewData["clave"] = "";
                ViewBag.Error = "La contraseña actual no coincide";
                return View();
            }
            else if (claveNueva != confirmarClave)
            {
                ViewData["clave"] = claveActual;
                TempData["idCliente"] = idCliente;
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }
            ViewData["clave"] = "";

            claveNueva = NegocioRecursos.ConvertirSHA256(claveNueva);
            string mensaje = string.Empty;
            bool respuesta = new NegocioCliente().CambiarClave(int.Parse(idCliente), claveNueva, out mensaje);

            if (respuesta)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = mensaje;
                TempData["idCliente"] = idCliente;
                return View();
            }


        }

        public ActionResult CerrarSesion()
        {
            Session["cliente"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Acceso");
        }
    }
}