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
        public SqlCommand comando()
        {
            return Comando;
        }
        public SqlConnection conexion()
        {
            return Conexion;
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
        public void ejecutarAccionEliminar(out bool resultado)
        {
            Comando.Connection = Conexion;
            try
            {
                Conexion.Open();
                Comando.ExecuteNonQuery();
                resultado = Comando.ExecuteNonQuery() > 0 ? true : false;
                Comando.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
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



       

        
        
       

    }
}
