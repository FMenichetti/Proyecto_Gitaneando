using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Data;
using System.Data.SqlClient;
using CapaDominio;
using System.Globalization;

namespace CapaDatos
{
    public class DatosProducto
    {


        public List<Producto> listar()
        {
            AccesoDatos datos = new AccesoDatos();
            List<Producto> lista = new List<Producto>();
            StringBuilder str = new StringBuilder();
            str.Append(" Select p.IdProducto, p.Nombre, p.Descripcion , ");
            str.Append(" m.IdMarcas, m.Descripcion [DescMarca], ");
            str.Append(" c.IdCategoria, c.Descripcion [descCategoria] , ");
            str.Append(" p.Precio, p.Stock, p.UrlImagen, p.NombreImagen, p.Activo ");
            str.Append(" from Producto p ");
            str.Append(" inner join Marcas m on m.IdMarcas = p.IdMarca ");
            str.Append(" inner join Categoria c on c.IdCategoria = p.IdCategoria ");
            try
            {
                datos.setearConsulta(str.ToString());
                datos.ejecutarLectura();
                while (datos.lector.Read())
                {
                    Producto Producto = new Producto();
                    Producto.IdProducto = int.Parse(datos.lector["IdProducto"].ToString());
                    Producto.Nombre = datos.lector["Descripcion"].ToString();
                    Producto.Descripcion = datos.lector["Descripcion"].ToString();
                    Producto.oMarca = new Marca();
                    Producto.oMarca.IdMarca = Convert.ToInt32(datos.lector["IdMarcas"]);
                    Producto.oMarca.Descripcion = datos.lector["DescMarca"].ToString();
                    Producto.oCategoria = new Categoria();
                    Producto.oCategoria.IdCategoria = Convert.ToInt32(datos.lector["IdCategoria"]);
                    Producto.oCategoria.Descripcion = datos.lector["DescCategoria"].ToString();
                    Producto.Precio = Convert.ToDecimal(datos.lector["Precio"], new CultureInfo("es-AR"));
                    Producto.Stock= Convert.ToInt32(datos.lector["Stock"]);
                    Producto.UrlImagen= datos.lector["UrlImagen"].ToString();
                    Producto.NombreImagen= datos.lector["NombreImagen"].ToString();
                    Producto.Activo = Convert.ToBoolean(datos.lector["Activo"]);
                    lista.Add(Producto);

                }
                return lista;
            }
            catch (Exception ex)
            {
                return lista = new List<Producto>();
                throw ex;
            }
            finally { datos.cerrarConexion(); }
        }

        public int RegistrarProducto_sp(Producto Producto, out string mensaje)
        {
            int idAutogenerado = 0;
            mensaje = string.Empty;
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearProcedimiento("sp_RegistrarProducto");
                datos.setearParametro("@Nombre", Producto.Nombre);
                datos.setearParametro("@Descripcion", Producto.Descripcion);
                datos.setearParametro("@IdCategoria", Producto.oCategoria.IdCategoria);
                datos.setearParametro("@IdMarca", Producto.oMarca.IdMarca);
                datos.setearParametro("@Precio", Producto.Precio);
                datos.setearParametro("@Stock", Producto.Stock);
                datos.setearParametro("@Activo", Producto.Activo);
                datos.comando().Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                datos.comando().Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                datos.ejecutarAccion();

                idAutogenerado = int.Parse(datos.comando().Parameters["Resultado"].Value.ToString());
                mensaje = datos.comando().Parameters["Mensaje"].Value.ToString();
            }
            catch (Exception ex)
            {
                idAutogenerado = 0;
                mensaje = ex.Message;

            }
            datos.cerrarConexion();
            return idAutogenerado;
        }

        public bool EditarProducto_Sp(Producto Producto, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("sp_EditarProducto");
                datos.setearParametro("@IdProducto", Producto.IdProducto);
                datos.setearParametro("@Nombre", Producto.Nombre);
                datos.setearParametro("@Descripcion", Producto.Descripcion);
                datos.setearParametro("@IdCategoria", Producto.oCategoria.IdCategoria);
                datos.setearParametro("@IdMarca", Producto.oMarca.IdMarca);
                datos.setearParametro("@Precio", Producto.Precio);
                datos.setearParametro("@Stock", Producto.Stock);
                datos.setearParametro("@Activo", Producto.Activo);
                datos.comando().Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
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

        public bool EliminarProducto_sp(int id, out string mensaje)
        {

            bool resultado = false;
            mensaje = string.Empty;

            AccesoDatos datos = new AccesoDatos();
            try
            {

                datos.setearProcedimiento("sp_EliminarProducto");
                datos.setearParametro("@IdProducto", id);
                datos.comando().Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                datos.comando().Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                datos.comando().Connection = datos.conexion();
                datos.conexion().Open();
                resultado = datos.comando().ExecuteNonQuery() > 0 ? true : false;

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

        public bool GuardarDatosImagen_sp(Producto Producto, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("Update Producto set UrlImagen = @UrlImagen, NombreImagen = @NombreImagen where IdProducto = @IdProducto");
                datos.setearParametro("@UrlImagen", Producto.UrlImagen);
                datos.setearParametro("@NombreImagen", Producto.NombreImagen);
                datos.setearParametro("@IdProducto", Producto.IdProducto);
                datos.comando().Connection = datos.conexion();
                datos.conexion().Open();
                if (datos.comando().ExecuteNonQuery() > 0)
                {
                    resultado = true;
                }
                else 
                {
                    resultado = false;
                    mensaje = "No se pudo actualizar imagen";
                }
 
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;

            }
            datos.cerrarConexion();
            return resultado;
        }


    }
}
