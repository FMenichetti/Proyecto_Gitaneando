using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDominio;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace CapaDatos
{
    public class DatosDashboard
    {
        public Dashboard verDashboard()
        {
            AccesoDatos datos = new AccesoDatos();
            Dashboard objeto = new Dashboard();
            try
            {
                datos.setearProcedimiento("sp_ReporteDashboard");
                datos.ejecutarLectura();
                while (datos.lector.Read())
                {
                    objeto.TotalClientes = Convert.ToInt32(datos.lector["TotalCliente"]);
                    objeto.TotalProductos = Convert.ToInt32(datos.lector["TotalProducto"]);
                    objeto.TotalVentas = Convert.ToInt32(datos.lector["TotalVenta"]);
                }
                return objeto;
            }
            catch (Exception)
            {
                objeto = new Dashboard();
                return objeto;
            }
            finally { datos.cerrarConexion(); }
        }


        public List<ReporteVentas> listarVentas(string fechaInicio, string fechaFin, string idTransaccion)
        {
            AccesoDatos datos = new AccesoDatos();
            List<ReporteVentas> lista = new List<ReporteVentas>();
            try
            {
                datos.setearProcedimiento("sp_ReporteVentas");
                datos.setearParametro("@fechaInicio", fechaInicio);
                datos.setearParametro("@fechaFin", fechaFin);
                datos.setearParametro("@idTransaccion", idTransaccion);


                datos.ejecutarLectura();
                while (datos.lector.Read())
                {
                    ReporteVentas reporte = new ReporteVentas();
                    reporte.Cantidad = Convert.ToInt32(datos.lector["Cantidad"].ToString());
                    reporte.Cliente = datos.lector["Cliente"].ToString();
                    reporte.FechaVenta = datos.lector["FechaVenta"].ToString();
                    reporte.IdTransaccion = datos.lector["IdTransaccion"].ToString();
                    reporte.Precio = Convert.ToDecimal(datos.lector["Precio"], new CultureInfo("es-AR"));
                    reporte.Producto = datos.lector["Producto"].ToString();
                    reporte.Total = Convert.ToDecimal(datos.lector["Total"].ToString(), new CultureInfo("es-AR"));
                    lista.Add(reporte);

                }
                return lista;
            }
            catch (Exception)
            {
                return lista = new List<ReporteVentas>();

            }
            finally { datos.cerrarConexion(); }
        }

    }
}
