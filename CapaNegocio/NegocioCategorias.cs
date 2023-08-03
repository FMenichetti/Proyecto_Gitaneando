using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaDominio;

namespace CapaNegocio
{
    public class NegocioCategorias
    {

         DatosCategoria datos = new DatosCategoria();

        public List<Categoria> Listar()
        {
            return datos.listar();
        }

        public int RegistrarCategoria_sp(Categoria categoria, out string mensaje)
        {
            mensaje = string.Empty;

            if (string.IsNullOrEmpty(categoria.Descripcion) || string.IsNullOrWhiteSpace(categoria.Descripcion))
                mensaje = "El campo de Categoria no puede estar vacio"; 

            if (string.IsNullOrEmpty(mensaje))
            {

                return datos.RegistrarCategoria_sp(categoria, out mensaje);
            }
            else
            {
                return 0;
            }
        }

        public bool EditarCategoria_Sp(Categoria categoria, out string mensaje)
        {
            mensaje = string.Empty;

            if (string.IsNullOrEmpty(categoria.Descripcion) || string.IsNullOrWhiteSpace(categoria.Descripcion))
                mensaje = "El campo de categoria no puede estar vacio";

            if (string.IsNullOrEmpty(mensaje))
            {

                return datos.EditarCategoria_Sp(categoria, out mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool EliminarCategoria_sp(int id, out string mensaje)
        {
            return datos.EliminarCategoria_sp(id, out mensaje);
        }
    }
}
