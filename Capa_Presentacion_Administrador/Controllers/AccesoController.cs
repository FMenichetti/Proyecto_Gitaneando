﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CapaDominio;
using CapaNegocio;

namespace Capa_Presentacion_Administrador.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CambiarClave()
        {
            return View();
        }
        public ActionResult Reestablecer()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
            Usuario user = new Usuario();
            user = new NegocioUsuarios().Listar().Where(i => i.Email == correo && i.Pass == NegocioRecursos.ConvertirSHA256(clave)).FirstOrDefault();

            if (user == null)//Si no encuentra usuario
            {
                ViewBag.error = "Correo o contraseña incorrecta";
                return View();
            }
            else
            {
                if (user.Restablecer)
                {
                    TempData["idUsuario"] = user.IdUsuario;
                    return RedirectToAction("CambiarClave");
                }

                ViewBag.error = null;
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult CambiarClave(string idUsuario, string claveActual, string claveNueva, string confirmarClave)
        {
            Usuario user = new Usuario();
            user = new NegocioUsuarios().Listar().Where(i => i.IdUsuario == int.Parse(idUsuario)).FirstOrDefault();
            if (user.Pass != NegocioRecursos.ConvertirSHA256(claveActual))
            {
                TempData["idUsuario"] = idUsuario;
                ViewData["clave"] = "";
                ViewBag.Error = "La contraseña actual no coincide";
                return View();
            }
            else if (claveNueva != confirmarClave)
            {
                ViewData["clave"] = claveActual;
                TempData["idUsuario"] = idUsuario;
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }
            ViewData["clave"] = "";

            claveNueva = NegocioRecursos.ConvertirSHA256(claveNueva);
            string mensaje = string.Empty;
            bool respuesta = new NegocioUsuarios().CambiarClave(int.Parse(idUsuario), claveNueva, out mensaje);

            if (respuesta)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = mensaje;
                TempData["idUsuario"] = idUsuario;
                return View();
            }

        }
    }
}