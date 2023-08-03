using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDominio;
using System.Data.SqlClient;
using System.Data;

namespace CapaDatos
{
    public class DatosUsuario
    {
        public List<Usuario> listar()
        {
            AccesoDatos datos = new AccesoDatos();
            List<Usuario> lista = new List<Usuario>();
            try
            {
                datos.setearConsulta("select IdUsuario, Nombre, Apellido, Email, Pass, Restablecer, Activo from Usuario");
                datos.ejecutarLectura();
                while (datos.lector.Read())
                {
                    Usuario usuario = new Usuario();
                    usuario.IdUsuario = int.Parse(datos.lector["IdUsuario"].ToString());
                    usuario.Nombre = datos.lector["Nombre"].ToString();
                    usuario.Apellido = datos.lector["Apellido"].ToString();
                    usuario.Email = datos.lector["Email"].ToString();
                    usuario.Pass = datos.lector["Pass"].ToString();
                    usuario.Restablecer = bool.Parse(datos.lector["Restablecer"].ToString());
                    usuario.Activo = bool.Parse(datos.lector["Activo"].ToString());
                    lista.Add(usuario);

                }
                return lista;
            }
            catch (Exception)
            {
                return lista = new List<Usuario>();

            }
            finally { datos.cerrarConexion(); }
        }

        public int RegistrarUsuarioConSP(Usuario user, out string mensaje)
        {
            int idAutogenerado = 0;
            mensaje = string.Empty;
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("sp_RegistrarUsuario");
                datos.setearParametro("@Nombre", user.Nombre);
                datos.setearParametro("@Apellido", user.Apellido);
                datos.setearParametro("@Email", user.Email);
                datos.setearParametro("@Pass", user.Pass);
                datos.setearParametro("@Activo", user.Activo);
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

        public bool EditarUsuarioConSp(Usuario user, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("sp_EditarUsuario");
                datos.setearParametro("@IdUsuario", user.IdUsuario);
                datos.setearParametro("@Nombre", user.Nombre);
                datos.setearParametro("@Apellido", user.Apellido);
                datos.setearParametro("@Email", user.Email);
                datos.setearParametro("@Activo", user.Activo);
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

        public bool Eliminar(int id, out string mensaje)
        {
            AccesoDatos datos = new AccesoDatos();
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                datos.setearConsulta("Delete from Usuario where IdUsuario= @Id");
                datos.setearParametro("@Id", id);
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

        public bool CambiarClave(int idUsuario, string nuevaClave, out string mensaje)
        {
            AccesoDatos datos = new AccesoDatos();
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                datos.setearConsulta("update Usuario set pass = @nuevaClave, Restablecer = 0 where idUsuario = @id");
                datos.setearParametro("@Id", idUsuario);
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

        public bool RestablecerClave(int idUsuario, string Clave, out string mensaje)
        {
            AccesoDatos datos = new AccesoDatos();
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                datos.setearConsulta("update Usuario set pass = @Clave, Restablecer = 1 where id = @id");
                datos.setearParametro("@Id", idUsuario);
                datos.setearParametro("@nuevaClave", Clave);
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
