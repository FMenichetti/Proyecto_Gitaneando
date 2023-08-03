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
    public class DatosMarca
    {

        public List<Marca> listar()
        {
            AccesoDatos datos = new AccesoDatos();
            List<Marca> lista = new List<Marca>();
            try
            {
                datos.setearConsulta("select IdMarcas, Descripcion, Activo  from Marcas");
                datos.ejecutarLectura();
                while (datos.lector.Read())
                {
                    Marca marca = new Marca();
                    marca.IdMarca = int.Parse(datos.lector["IdMarcas"].ToString());
                    marca.Descripcion = datos.lector["Descripcion"].ToString();
                    marca.Activo = bool.Parse(datos.lector["Activo"].ToString());
                    lista.Add(marca);

                }
                return lista;
            }
            catch (Exception)
            {
                return lista = new List<Marca>();

            }
            finally { datos.cerrarConexion(); }
        }

        public int RegistrarMarca_sp(Marca marca, out string mensaje)
        {
            int idAutogenerado = 0;
            mensaje = string.Empty;
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearProcedimiento("sp_RegistrarMarca");
                datos.setearParametro("@Descripcion", marca.Descripcion);
                datos.setearParametro("@Activo", marca.Activo);
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

        public bool EditarMarca_Sp(Marca marca, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("sp_EditarMarca");
                datos.setearParametro("@IdMarca", marca.IdMarca);
                datos.setearParametro("@Descripcion", marca.Descripcion);
                datos.setearParametro("@Activo", marca.Activo);
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

        public bool EliminarMarca_sp(int id, out string mensaje)
        {

            bool resultado = false;
            mensaje = string.Empty;

            AccesoDatos datos = new AccesoDatos();
            try
            {

                datos.setearProcedimiento("sp_EliminarMarca");
                datos.setearParametro("@IdMarca", id);
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
