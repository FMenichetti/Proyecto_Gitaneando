using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaNegocio;
using CapaDominio;
using System.Data;
using ClosedXML.Excel;
using System.IO;

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
            return Json(new { data = listaUsuarios }, JsonRequestBehavior.AllowGet);

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

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EliminarUsuario(int id)
        {
            object resultado;
            string mensaje = string.Empty;
            try
            {
                resultado = new NegocioUsuarios().Eliminar(id, out mensaje);

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult verDashboard()
        {

            Dashboard objeto = new NegocioDashboard().verDashboard();

            return Json(new { resultado = objeto }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult listarVentas(string fechaInicio, string fechaFin, string idTransaccion)
        {
            List<ReporteVentas> lista = new List<ReporteVentas>();


            lista = new NegocioDashboard().listarVentas(fechaInicio, fechaFin, idTransaccion);

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public FileResult ExportarVenta(string fechaInicio, string fechaFin, string idTransaccion)
        {
            List<ReporteVentas> lista = new List<ReporteVentas>();
            lista = new NegocioDashboard().listarVentas(fechaInicio,fechaFin,idTransaccion);

            DataTable dt = new DataTable();

            dt.Locale = new System.Globalization.CultureInfo("es-AR");
            dt.Columns.Add("Fecha Venta", typeof(string));
            dt.Columns.Add("Cliente", typeof(string));
            dt.Columns.Add("Producto", typeof(string));
            dt.Columns.Add("Precio", typeof(decimal));
            dt.Columns.Add("Cantidad", typeof(int));
            dt.Columns.Add("Total", typeof(decimal));
            dt.Columns.Add("Id Transaccion", typeof(string));

            foreach (ReporteVentas reporte in lista)
            {
                dt.Rows.Add(new object[] {
                 reporte.FechaVenta,
                 reporte.Cliente,
                 reporte.Producto,
                 reporte.Precio,
                 reporte.Cantidad,
                 reporte.Total,
                 reporte.IdTransaccion
                });
            }

            dt.TableName = "Datos";

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "aplication/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reporte Venta" + DateTime.Now.ToString() + ".xlsx");
                }
            }
        }
    }
}