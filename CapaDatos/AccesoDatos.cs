using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDominio;


namespace CapaDatos
{
    public class AccesoDatos
    {
        private SqlConnection Conexion { get; set; }
        private SqlCommand Comando { get; set; }
        private SqlDataReader Lector { get; set; }
        public SqlDataReader lector
        {
            get { return Lector; }
        }
        public AccesoDatos()
        {
            //Maxi//Conexion = new SqlConnection(ConfigurationManager.AppSettings["cadenaConexion"]);
            Conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["cadenaConexion"].ToString());
            Comando = new SqlCommand();
        }
        public void setearConsulta(string consulta)
        {
            Comando.CommandType = System.Data.CommandType.Text;
            Comando.CommandText = consulta;
        }
        public void setearConsultaConSP(string consulta)
        {
            Comando.CommandType = System.Data.CommandType.StoredProcedure;
            Comando.CommandText = consulta;
        }
        public void ejecutarLectura()
        {
            Comando.Connection = Conexion;
            try
            {
                Conexion.Open();
                Lector = Comando.ExecuteReader();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public void ejecutarAccion()
        {
            Comando.Connection = Conexion;
            try
            {
                Conexion.Open();
                Comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public void cerrarConexion()
        {
            if (Lector != null)
            {
                Lector.Close();
                Conexion.Close();
            }
        }

        public void setearProcedimiento(string sp)
        {
            Comando.CommandType = System.Data.CommandType.StoredProcedure;
            Comando.CommandText = sp;
        }
        public void setearParametro(string nombre, Object valor)
        {
            Comando.Parameters.AddWithValue(nombre, valor);
        }



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
                Comando.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                Comando.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                datos.ejecutarAccion();

                idAutogenerado = int.Parse(Comando.Parameters["Resultado"].Value.ToString());
                mensaje = Comando.Parameters["Mensaje"].Value.ToString();
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
                Comando.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                Comando.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                datos.ejecutarAccion();

                resultado = bool.Parse(Comando.Parameters["Resultado"].Value.ToString());
                mensaje = Comando.Parameters["Mensaje"].Value.ToString();
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
                datos.setearConsulta("Delete from Usuario where id= @Id");
                datos.setearParametro("@Id", id);
                Conexion.Open();
                resultado = Comando.ExecuteNonQuery() > 0 ? true : false;

                
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
