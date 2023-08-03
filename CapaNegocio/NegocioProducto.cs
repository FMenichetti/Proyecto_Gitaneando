using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaDominio;

namespace CapaNegocio
{
    public class NegocioProducto
    {


        DatosProducto datos = new DatosProducto();

        public List<Producto> Listar()
        {
            return datos.listar();
        }

        public int RegistrarProducto_sp(Producto producto, out string mensaje)
        {
            mensaje = string.Empty;

            if (string.IsNullOrEmpty(producto.Descripcion) || string.IsNullOrWhiteSpace(producto.Descripcion))
                mensaje = "El campo Descripcion no puede estar vacio";

            else if (string.IsNullOrEmpty(producto.Nombre) || string.IsNullOrWhiteSpace(producto.Nombre))
                mensaje = "El campo Nombre no puede estar vacio";

            else if (string.IsNullOrEmpty(producto.oCategoria.Descripcion) || string.IsNullOrWhiteSpace(producto.oCategoria.Descripcion))
                mensaje = "El campo Categoria no puede estar vacio";

            else if (string.IsNullOrEmpty(producto.oMarca.Descripcion) || string.IsNullOrWhiteSpace(producto.oMarca.Descripcion))
                mensaje = "El campo Marca no puede estar vacio";

            else if (producto.Precio == 0 )
                mensaje = "El campo Precio no puede estar vacio";

            else if (producto.Stock == 0)
                mensaje = "El campo Stock no puede estar vacio";

            if (string.IsNullOrEmpty(mensaje))
            {

                return datos.RegistrarProducto_sp(producto, out mensaje);
            }
            else
            {
                return 0;
            }
        }

        public bool EditarProducto_Sp(Producto producto, out string mensaje)
        {
            mensaje = string.Empty;


            if (string.IsNullOrEmpty(producto.Descripcion) || string.IsNullOrWhiteSpace(producto.Descripcion))
                mensaje = "El campo Descripcion no puede estar vacio";

            else if (string.IsNullOrEmpty(producto.Nombre) || string.IsNullOrWhiteSpace(producto.Nombre))
                mensaje = "El campo Nombre no puede estar vacio";

            else if (string.IsNullOrEmpty(producto.oCategoria.Descripcion) || string.IsNullOrWhiteSpace(producto.oCategoria.Descripcion))
                mensaje = "El campo Categoria no puede estar vacio";

            else if (string.IsNullOrEmpty(producto.oMarca.Descripcion) || string.IsNullOrWhiteSpace(producto.oMarca.Descripcion))
                mensaje = "El campo Marca no puede estar vacio";

            else if (producto.Precio == 0)
                mensaje = "El campo Precio no puede estar vacio";

            else if (producto.Stock == 0)
                mensaje = "El campo Stock no puede estar vacio";

            if (string.IsNullOrEmpty(mensaje))
            {

                return datos.EditarProducto_Sp(producto, out mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool EliminarProducto_sp(int id, out string mensaje)
        {
            return datos.EliminarProducto_sp(id, out mensaje);
        }

        public bool GuardarDatosImagen_sp(Producto producto, out string mensaje)
        {
            return datos.GuardarDatosImagen_sp(producto ,out mensaje );
        }
    }
}
