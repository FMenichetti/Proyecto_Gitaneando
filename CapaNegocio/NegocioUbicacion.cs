using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaDominio;

namespace CapaNegocio
{
    public class NegocioUbicacion
    {
        private DatosUbicacion datos = new DatosUbicacion();

        public List<Provincia> ObtenerProvincia()
        {
            return datos.ObtenerProvincia();
        }

        public List<Ciudad> ObtenerCiudad(string idProvincia)
        {
            return datos.ObtenerCiudad(idProvincia);
        }

        public List<Barrio> ObtenerBarrio(string idCiudad, string idProvincia)
        {
            return datos.ObtenerBarrio(idCiudad, idProvincia);
        }
    }
}
