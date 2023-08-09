using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaDominio;

namespace CapaNegocio
{
    public class NegocioCarrito
    {
        DatosCarrito datos = new DatosCarrito();

        public bool ExisteCarrito(int idCliente, int idProducto)
        {
            return datos.ExisteCarrito(idCliente,idProducto);
        }

        public bool OperacionCarrito(int idCliente, int idProducto, bool sumar, out string mensaje)
        {
            return datos.OperacionCarrito(idCliente, idProducto, sumar, out mensaje);
        }

        public int CantidadEnCarrito(int idCliente) {
            return datos.CantidadEnCarrito(idCliente);
        }

        public List<Carrito> ListarProducto(int idCliente)
        {
            return datos.ListarProducto(idCliente);
        }

        public bool EliminarCarrito(int idCliente, int idProducto)
        {
            return datos.EliminarCarrito(idCliente, idProducto);
        }
    }
}
