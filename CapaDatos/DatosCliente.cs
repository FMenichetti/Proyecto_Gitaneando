using CapaDominio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class DatosCliente
    {
        public int RegistrarClienteConSP(Cliente user, out string mensaje)
        {
            int idAutogenerado = 0;
            mensaje = string.Empty;
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("sp_RegistrarCliente");
                datos.setearParametro("@Nombre", user.Nombre);
                datos.setearParametro("@Apellido", user.Apellido);
                datos.setearParametro("@Email", user.Email);
                datos.setearParametro("@Pass", user.Pass);
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
        public List<Cliente> listar()
        {
            AccesoDatos datos = new AccesoDatos();
            List<Cliente> lista = new List<Cliente>();
            try
            {
                datos.setearConsulta("select IdCliente, Nombre, Apellido, Email, Pass, Restablecer from Cliente");
                datos.ejecutarLectura();
                while (datos.lector.Read())
                {
                    Cliente Cliente = new Cliente();
                    Cliente.IdCliente = int.Parse(datos.lector["IdCliente"].ToString());
                    Cliente.Nombre = datos.lector["Nombre"].ToString();
                    Cliente.Apellido = datos.lector["Apellido"].ToString();
                    Cliente.Email = datos.lector["Email"].ToString();
                    Cliente.Pass = datos.lector["Pass"].ToString();
                    Cliente.Restablecer = bool.Parse(datos.lector["Restablecer"].ToString());
                    lista.Add(Cliente);

                }
                return lista;
            }
            catch (Exception)
            {
                return lista = new List<Cliente>();

            }
            finally { datos.cerrarConexion(); }
        }

        public bool CambiarClave(int idCliente, string nuevaClave, out string mensaje)
        {
            AccesoDatos datos = new AccesoDatos();
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                datos.setearConsulta("update Cliente set pass = @nuevaClave, Restablecer = 0 where idCliente = @id");
                datos.setearParametro("@Id", idCliente);
                datos.setearParametro("@nuevaClave", nuevaClave);
                datos.comando().Connection = datos.conexion();
                datos.conexion().Open();
                resultado = datos.comando().ExecuteNonQuery() > 0 ? true : false;


            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }
            datos.cerrarConexion();
            return resultado;
        }

        public bool RestablecerClave(int idCliente, string clave, out string mensaje)
        {
            AccesoDatos datos = new AccesoDatos();
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                datos.setearConsulta("update Cliente set pass = @Clave, Restablecer = 1 where idCliente = @id");
                datos.setearParametro("@Id", idCliente);
                datos.setearParametro("@Clave", clave);
                datos.comando().Connection = datos.conexion();
                datos.conexion().Open();
                resultado = datos.comando().ExecuteNonQuery() > 0 ? true : false;


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
