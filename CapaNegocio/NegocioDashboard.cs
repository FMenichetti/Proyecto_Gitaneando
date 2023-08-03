using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaDominio;

namespace CapaNegocio
{
    public class NegocioDashboard
    {

        DatosDashboard datos = new DatosDashboard();

        public Dashboard verDashboard()
        {
            return datos.verDashboard();
        }

        public List<ReporteVentas> listarVentas(string fechaInicio, string fechaFin, string idTransaccion)
        {
            return datos.listarVentas(fechaInicio, fechaFin, idTransaccion);
        }

    }
}
