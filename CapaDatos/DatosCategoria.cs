using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CapaDominio;

namespace CapaDatos
{
    public class DatosCategoria
    {
        public List<Categoria> listar()
        {
            AccesoDatos datos = new AccesoDatos();
            List<Categoria> lista = new List<Categoria>();
            try
            {
                datos.setearConsulta("select IdCategoria, Descripcion, Activo  from Categoria");
                datos.ejecutarLectura();
                while (datos.lector.Read())
                {
                    Categoria categoria = new Categoria();
                    categoria.IdCategoria = int.Parse(datos.lector["IdCategoria"].ToString());
                    categoria.Descripcion = datos.lector["Descripcion"].ToString();
                    categoria.Activo = bool.Parse(datos.lector["Activo"].ToString());
                    lista.Add(categoria);

                }
                return lista;
            }
            catch (Exception)
            {
                return lista = new List<Categoria>();

            }
            finally { datos.cerrarConexion(); }
        }

        public int RegistrarCategoria_sp(Categoria categoria, out string mensaje)
        {
            int idAutogenerado = 0;
            mensaje = string.Empty;
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearProcedimiento("sp_RegistrarCategoria");
                datos.setearParametro("@Descripcion", categoria.Descripcion);
                datos.setearParametro("@Activo", categoria.Activo);
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

        public bool EditarCategoria_Sp(Categoria categoria, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("sp_EditarCategoria");
                datos.setearParametro("@IdCategoria", categoria.IdCategoria);
                datos.setearParametro("@Descripcion", categoria.Descripcion);
                datos.setearParametro("@Activo", categoria.Activo);
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

        public bool EliminarCategoria_sp(int id, out string mensaje)
        {
            
            bool resultado = false;
            mensaje = string.Empty;
            
            AccesoDatos datos = new AccesoDatos();
            try
            {
               
                datos.setearProcedimiento("sp_EliminarCategoria");
                datos.setearParametro("@IdCategoria", id);
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

    }
}
