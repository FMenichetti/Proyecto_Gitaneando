using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDominio;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class DatosUbicacion
    {

        public List<Provincia> ObtenerProvincia()
        {
            AccesoDatos datos = new AccesoDatos();
            List<Provincia> lista = new List<Provincia>();
            try
            {
                datos.setearConsulta("select * from provincia");
                datos.ejecutarLectura();
                while (datos.lector.Read())
                {
                    Provincia provincia = new Provincia();
                    provincia.IdProvincia = datos.lector["IdProvincia"].ToString();
                    provincia.Descripcion = datos.lector["Descripcion"].ToString();
                   
                    lista.Add(provincia);

                }
                return lista;
            }
            catch (Exception)
            {
                return lista = new List<Provincia>();

            }
            finally { datos.cerrarConexion(); }
        }

        public List<Ciudad> ObtenerCiudad(string idProvincia)
        {
            AccesoDatos datos = new AccesoDatos();
            List<Ciudad> lista = new List<Ciudad>();
            try
            {
                datos.setearConsulta("select * from Ciudad where IdProvincia = @IdProvincia");
                datos.setearParametro("@IdProvincia", idProvincia);
                datos.ejecutarLectura();
                while (datos.lector.Read())
                {
                    Ciudad ciudad = new Ciudad();
                    ciudad.IdCiudad = datos.lector["IdCiudad"].ToString();
                    ciudad.Descripcion = datos.lector["Descripcion"].ToString();

                    lista.Add(ciudad);

                }
                return lista;
            }
            catch (Exception)
            {
                return lista = new List<Ciudad>();

            }
            finally { datos.cerrarConexion(); }
        }

        public List<Barrio> ObtenerBarrio(string idCiudad, string idProvincia)
        {
            AccesoDatos datos = new AccesoDatos();
            List<Barrio> lista = new List<Barrio>();
            try
            {
                datos.setearConsulta("select * from Barrio where IdCiudad = @IdCiudad and IdProvincia = @IdProvincia");
                datos.setearParametro("@IdCiudad", idCiudad);
                datos.setearParametro("@IdProvincia", idProvincia);
                datos.ejecutarLectura();
                while (datos.lector.Read())
                {
                    Barrio Barrio = new Barrio();
                    Barrio.IdBarrio = datos.lector["IdBarrio"].ToString();
                    Barrio.Descripcion = datos.lector["Descripcion"].ToString();

                    lista.Add(Barrio);

                }
                return lista;
            }
            catch (Exception)
            {
                return lista = new List<Barrio>();

            }
            finally { datos.cerrarConexion(); }
        }
    }
}
