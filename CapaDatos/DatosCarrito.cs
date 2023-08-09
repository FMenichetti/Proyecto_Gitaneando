using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDominio;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace CapaDatos
{
    public class DatosCarrito
    {


        public bool ExisteCarrito(int idCliente, int idProducto)
        {
            bool resultado = true;

            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("sp_ExisteCarrito");
                datos.setearParametro("@IdCliente", idCliente);
                datos.setearParametro("@IdProducto", idProducto);
                datos.comando().Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                datos.ejecutarAccion();

                resultado = Convert.ToBoolean(datos.comando().Parameters["Resultado"].Value);
            }
            catch (Exception ex)
            {
                resultado = false;

            }
            datos.cerrarConexion();
            return resultado;
        }

        public bool OperacionCarrito(int idCliente, int idProducto, bool sumar, out string mensaje)
        {
            bool resultado = true;
            mensaje = string.Empty;

            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("sp_OperacionCarrito");
                datos.setearParametro("@IdCliente", idCliente);
                datos.setearParametro("@IdProducto", idProducto);
                datos.setearParametro("@Sumar", sumar);
                datos.comando().Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                datos.comando().Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                datos.ejecutarAccion();

                resultado = Convert.ToBoolean(datos.comando().Parameters["Resultado"].Value);
                mensaje = datos.comando().Parameters["Mensaje"].Value.ToString();
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;

            }
            datos.cerrarConexion();
            return resultado;
        }

        public int CantidadEnCarrito(int idCliente)
        {
            AccesoDatos datos = new AccesoDatos();
            int resultado = 0;

            try
            {
                datos.setearConsulta("Select count (*) from Carrito where IdCliente= @IdCliente");
                datos.setearParametro("@IdCliente", idCliente);
                datos.comando().Connection = datos.conexion();
                datos.conexion().Open();
                resultado = Convert.ToInt32(datos.comando().ExecuteScalar());


            }
            catch (Exception ex)
            {
                resultado = 0;
            }
            datos.cerrarConexion();
            return resultado;
        }

        public List<Carrito> ListarProducto(int idCliente)
        {
            AccesoDatos datos = new AccesoDatos();
            List<Carrito> lista = new List<Carrito>();
           
            try
            {
                datos.setearConsulta("select * from fn_ObtenerCarritoCliente(@IdCliente)");
                datos.setearParametro("@IdCliente", idCliente);
                datos.ejecutarLectura();
                while (datos.lector.Read())
                {

                    Carrito carrito = new Carrito();
                    carrito.oProducto = new Producto();
                    carrito.oProducto.IdProducto = Convert.ToInt32(datos.lector["IdProducto"]);
                    carrito.oProducto.Nombre = datos.lector["Nombre"].ToString();
                    carrito.oProducto.Precio = Convert.ToDecimal(datos.lector["Precio"], new CultureInfo("es-AR"));
                    carrito.oProducto.UrlImagen = datos.lector["UrlImagen"].ToString();
                    carrito.oProducto.NombreImagen = datos.lector["NombreImagen"].ToString();
                    carrito.oProducto.oMarca = new Marca();
                   //carrito.oProducto.oMarca.IdMarca = Convert.ToInt32(datos.lector["IdMarcas"]);
                    carrito.oProducto.oMarca.Descripcion = datos.lector["desMarca"].ToString();
                    carrito.Cantidad = Convert.ToInt32(datos.lector["Cantidad"]);
                    lista.Add(carrito);

                }
                return lista;
            }
            catch (Exception ex)
            {
                return lista = new List<Carrito>();
            
            }
            finally { datos.cerrarConexion(); }
        }

        public bool EliminarCarrito(int idCliente, int idProducto)
        {
            bool resultado = true;

            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("sp_EliminarCarrito");
                datos.setearParametro("@IdCliente", idCliente);
                datos.setearParametro("@IdProducto", idProducto);
                datos.comando().Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                datos.ejecutarAccion();

                resultado = Convert.ToBoolean(datos.comando().Parameters["Resultado"].Value);
            }
            catch (Exception ex)
            {
                resultado = false;

            }
            datos.cerrarConexion();
            return resultado;
        }
    }
}
