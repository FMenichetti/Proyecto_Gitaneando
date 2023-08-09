using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaDominio;
using CapaNegocio;
using Newtonsoft.Json;
using System.Globalization;
using System.Configuration;
using System.IO;

namespace Capa_Presentacion_Administrador.Controllers
{
    [Authorize]
    public class MantenimientoController : Controller
    {
        // GET: Mantenimiento
        public ActionResult Categorias()
        {
            return View();
        }
        public ActionResult Marcas()
        {
            return View();
        }
        public ActionResult Producto()
        {
            return View();
        }
        // -------------CATEGORIA------------------
        #region Categoria

        [HttpGet]
        public JsonResult ListarCategorias()
        {
            List<Categoria> listaCategorias = new List<Categoria>();
            listaCategorias = new NegocioCategorias().Listar();
            //Lo de arriba es lo mismo  pero esta abreviado
            //NegocioUsuarios negocioUsuarios = new NegocioUsuarios();
            //listaUsuarios = negocioUsuarios.Listar();
            return Json(new { data = listaCategorias }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GuardarCategoria(Categoria categoria)
        {
            object resultado;
            string mensaje = string.Empty;

            if (categoria.IdCategoria == 0)
            {

                resultado = new NegocioCategorias().RegistrarCategoria_sp(categoria, out mensaje);
            }
            else
            {
                resultado = new NegocioCategorias().EditarCategoria_Sp(categoria, out mensaje);

            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarCategoria(int id)
        {
            object resultado;
            string mensaje = string.Empty;
            try
            {
                resultado = new NegocioCategorias().EliminarCategoria_sp(id, out mensaje);

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        #endregion
        //-------------------MARCA--------------
        #region Marca
        [HttpGet]
        public JsonResult ListarMarca()
        {
            List<Marca> listaMarcas = new List<Marca>();
            listaMarcas = new NegocioMarcas().Listar();
            //Lo de arriba es lo mismo  pero esta abreviado
            //NegocioUsuarios negocioUsuarios = new NegocioUsuarios();
            //listaUsuarios = negocioUsuarios.Listar();
            return Json(new { data = listaMarcas }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GuardarMarca(Marca marca)
        {
            object resultado;
            string mensaje = string.Empty;

            if (marca.IdMarca == 0)
            {

                resultado = new NegocioMarcas().RegistrarMarca_sp(marca, out mensaje);
            }
            else
            {
                resultado = new NegocioMarcas().EditarMarca_Sp(marca, out mensaje);

            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarMarca(int id)
        {
            object resultado;
            string mensaje = string.Empty;
            try
            {
                resultado = new NegocioMarcas().EliminarMarca_sp(id, out mensaje);

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion
        //---------------PRODUCTO-------------

        #region Producto
        [HttpGet]
        public JsonResult ListarProducto()
        {
            List<Producto> listaProductos = new List<Producto>();
            listaProductos = new NegocioProducto().Listar();
            //Lo de arriba es lo mismo  pero esta abreviado
            //NegocioUsuarios negocioUsuarios = new NegocioUsuarios();
            //listaUsuarios = negocioUsuarios.Listar();
            return Json(new { data = listaProductos }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        //necesito probar xq recibo un string y lo transformo en producto y no recibo un producto
        public JsonResult GuardarProducto(string objeto, HttpPostedFileBase archivoImagen)
        {
            
            string mensaje = string.Empty;
            bool operacionExitosa = true;
            bool guardarImagenExito = true;
            

            Producto oProducto = new Producto();
            oProducto = JsonConvert.DeserializeObject<Producto>(objeto); //Convierto el json recibido en un producto
           
            //El precio en texto lo convierto a decimal segun cultura y por un lado contesto if y guardo el valor en precio
            if (decimal.TryParse(oProducto.precioTexto/*, NumberStyles.AllowDecimalPoint, new CultureInfo("es-AR")*/,out decimal precio))
            {
                oProducto.Precio = precio;
            }
            else
            {
                return Json(new { operacionExitosa = false, mensaje = "Formato erroneo, intente ##.##" }, JsonRequestBehavior.AllowGet);
            }

            if (oProducto.IdProducto == 0)
            {

                int idProductoGenerado = new NegocioProducto().RegistrarProducto_sp(oProducto, out mensaje);
                if (idProductoGenerado != 0) // Si fue exitoso el reggistro, actualizo el id
                {
                    oProducto.IdProducto = idProductoGenerado;
                }
                else
                {
                    operacionExitosa = false;
                }
            }
            else
            {   //si no es ==0 edito
                operacionExitosa = new NegocioProducto().EditarProducto_Sp(oProducto, out mensaje);

            }

            //LOGICA DE GUARDAR IMAGEN
            if (operacionExitosa)
            {
                if(archivoImagen != null)
                {
                    string rutaGuardar = ConfigurationManager.AppSettings["servidorFotos"];
                    string extension = Path.GetExtension(archivoImagen.FileName);
                    string nombreImagen = string.Concat(oProducto.IdProducto.ToString(), extension);

                    try
                    {
                        archivoImagen.SaveAs(Path.Combine(rutaGuardar, nombreImagen));
                    }
                    catch (Exception ex)
                    {
                        string msj = ex.Message;
                        guardarImagenExito = false;
                        
                    }
                    if (guardarImagenExito)
                    {
                        oProducto.UrlImagen = rutaGuardar;
                        oProducto.NombreImagen = nombreImagen;
                        bool rst = new NegocioProducto().GuardarDatosImagen_sp(oProducto, out mensaje);
                    }
                    else
                    {
                        mensaje = "Se guardo el producto, pero hubo un problema con la imagen";
                    }
                }
            }


            return Json(new { operacionExitosa = operacionExitosa,idGenerado = oProducto.IdProducto, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ImagenProducto(int id)// Con el Id busco el producto y con la ruta e imagen hago mi textbase64 y lo devuelvo en json
        {
            bool conversion;
            Producto oProducto = new NegocioProducto().Listar().Where(p => p.IdProducto == id).FirstOrDefault();
            string textoBase64 = NegocioRecursos.ConvertirBase64(Path.Combine(oProducto.UrlImagen, oProducto.NombreImagen), out conversion);

            return Json(new { conversion = conversion, 
                             textoBase64 = textoBase64,
                             extension = Path.GetExtension(oProducto.NombreImagen) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarProducto(int id)
        {
            object resultado;
            string mensaje = string.Empty;
            try
            {
                resultado = new NegocioProducto().EliminarProducto_sp(id, out mensaje);

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}